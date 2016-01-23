using System;
using System.Linq;
using System.Threading;
using Common.Events;
using Jobs.Context;
using Jobs.Exceptions;
using Jobs.Plugins;
using Jobs.States;
using Jobs.Tasks;
using Jobs.Tasks.Events;
using Logging;

namespace Jobs
{
    /// <summary>
    /// Handles the threaded work of executing tasks.
    /// </summary>
    internal sealed class Job : iJob
    {
        /// <summary>
        /// Logging
        /// </summary>
        private static readonly Logger _logger = Logger.Create(typeof (Job));

        /// <summary>
        /// Used to create contexts for the tasks.
        /// </summary>
        private readonly iJobContextFactory _jobContextFactory;

        /// <summary>
        /// Used to tell the thread to exit.
        /// </summary>
        private readonly AutoResetEvent _eventDelay;

        /// <summary>
        /// Used to report unhandled exception.
        /// </summary>
        private readonly iEventFactory _eventFactory;

        /// <summary>
        /// Allows an external resource to control the running state of the job.
        /// </summary>
        private readonly iJobState _jobState;

        /// <summary>
        /// The settings for the plug-in.
        /// </summary>
        private readonly PluginSettings _settings;

        /// <summary>
        /// True if the worker thread is running.
        /// </summary>
        private bool _isRunning;

        /// <summary>
        /// True if the job is suspended from performing tasks.
        /// </summary>
        private bool _isSuspended;

        /// <summary>
        /// The worker thread for the job.
        /// </summary>
        private Thread _worker;

        /// <summary>
        /// Executes all the tasks assigned to this job.
        /// </summary>
        private void PerformTasks()
        {
            JobContext context = null;

            try
            {
                Count++;

                context = _jobContextFactory.Create(_settings);
                foreach (iTaskEntry entry in Tasks)
                {
                    iEventRecorder recorder = entry.Recorder;

                    // keep running tasks even if it fails.
                    // tasks should be run independent of each other
                    try
                    {
                        context.setEventRecorder(recorder);
                        entry.Task.Execute(context, recorder);
                    }
                    catch (Exception err)
                    {
                        _logger.Exception(err);

                        iEventObject eventObj = _eventFactory.Create(eEVENT_SEVERITY.UNHANDLED, err);
                        recorder.Add(eventObj);

                        FireEvents.Action(JobError, ID, err);

                        Errors++;

                        if (err.GetType() == typeof (AbortJobException))
                        {
                            throw;
                        }

                        if (MaxErrors <= 0 || Errors != MaxErrors)
                        {
                            continue;
                        }

                        _isRunning = false;
                        _logger.Error("Job is aborting because it exceeded max allowed errors of {0}.", MaxErrors);
                    }
                }
            }
            finally
            {
                if (context != null)
                {
                    context.Dispose();
                }
            }
        }

        /// <summary>
        /// Worker thread to extract contact records.
        /// </summary>
        private void WorkerProc()
        {
            try
            {
                State = eSTATE.BUSY;
                _isRunning = true;
                bool firstTime = true;

                while (_isRunning && !_jobState.isFinished())
                {
                    State = eSTATE.BUSY;
                    TimeStamp = DateTime.Now;

                    if (!_isSuspended)
                    {
                        try
                        {
                            if (!firstTime || _jobState.TasksFirst())
                            {
                                PerformTasks();
                            }
                        }
                        finally
                        {
                            firstTime = false;
                        }
                    }

                    TimeSpan delay = _jobState.Delay();
                    if (delay.Ticks < 0)
                    {
                        throw new JobException("Negative delay value.");
                    }

                    TimeStamp = DateTime.Now.Add(delay);

                    if (!(delay.TotalSeconds > 0) || !_isRunning)
                    {
                        continue;
                    }

                    State = _isSuspended
                        ? eSTATE.SUSPENDED
                        : eSTATE.IDLE;

                    _eventDelay.WaitOne(delay);
                }
            }
            catch (Exception err)
            {
                _logger.Error("Job caused unhandled exception.");
                _logger.Exception(err);
                FireEvents.Action(JobError, ID, err);

                if (err.GetType() == typeof (AbortJobException))
                {
                    State = eSTATE.FAILED;
                }
            }
            finally
            {
                Finished();
            }

            _logger.Fine("Thread has ended");
        }

        /// <summary>
        /// Changes the state of the job to finished.
        /// </summary>
        private void Finished()
        {
            TimeStamp = DateTime.Now;
            if (State != eSTATE.FAILED)
            {
                State = eSTATE.FINISHED;
            }
            _isRunning = false;
            FireEvents.Action(JobFinish, ID);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Job(
            Guid pParentID,
            PluginSettings pSettings,
            string pPlugin,
            string pName,
            string pCode,
            iTaskCollection pTasks,
            iJobContextFactory pJobContextFactory,
            iEventFactory pEventFactory,
            iJobState pJobState,
            int pMaxErrors = 0)
        {
            _settings = pSettings;
            _jobContextFactory = pJobContextFactory;
            _jobState = pJobState;
            _eventDelay = new AutoResetEvent(false);

            _isRunning = false;
            _isSuspended = false;

            Tasks = pTasks;
            ParentID = pParentID;
            Plugin = pPlugin;
            Name = pName;
            Code = pCode;
            ID = Guid.NewGuid();
            State = eSTATE.NONE;

            _eventFactory = pEventFactory;

            Errors = 0;
            MaxErrors = pMaxErrors;
            TimeStamp = DateTime.Now;
            ThreadID = 0;
        }

        /// <summary>
        /// The max number of unexpected errors before the job stops.
        /// Set to zero for unlimited.
        /// </summary>
        public int MaxErrors { get; private set; }

        /// <summary>
        /// The name of the plug-in that created the job.
        /// </summary>
        public string Plugin { get; private set; }

        /// <summary>
        /// The name of the job.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Used to create tasks for the job.
        /// </summary>
        public iTaskCollection Tasks { get; private set; }

        /// <summary>
        /// A unique code value that identifies this job.
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// The number of times this job has executed.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// How many unexpected exceptions this job has raised.
        /// </summary>
        public int Errors { get; private set; }

        /// <summary>
        /// The current state of the job.
        /// </summary>
        public eSTATE State { get; private set; }

        /// <summary>
        /// The ID of the worker thread.
        /// </summary>
        public int ThreadID { get; private set; }

        /// <summary>
        /// This timestamp depends upon the current state of the job.
        /// </summary>
        public DateTime TimeStamp { get; private set; }

        /// <summary>
        /// A unique ID for the job.
        /// </summary>
        public Guid ID { get; private set; }

        /// <summary>
        /// The ID of the job that must finish before this job can start.
        /// </summary>
        public Guid ParentID { get; private set; }

        /// <summary>
        /// Fired when the job is created.
        /// </summary>
        public event Action<Guid> JobCreated;

        /// <summary>
        /// Called when the worker thread is started.
        /// </summary>
        public event Action<Guid> JobStart;

        /// <summary>
        /// Called when the worker thread exists.
        /// </summary>
        public event Action<Guid> JobFinish;

        /// <summary>
        /// Called if the tasks raise an exception.
        /// </summary>
        public event Action<Guid, Exception> JobError;

        /// <summary>
        /// Clears the event record for all tasks.
        /// </summary>
        public void Clear()
        {
            lock (Tasks)
            {
                foreach (iTaskEntry entry in Tasks)
                {
                    entry.Recorder.Clear();
                }
            }
        }

        /// <summary>
        /// Clears the event record for a single task.
        /// </summary>
        /// <param name="pTaskID">The task ID to clear.</param>
        public void Clear(Guid pTaskID)
        {
            lock (Tasks)
            {
                if (!Tasks.ContainsTask(pTaskID))
                {
                    return;
                }
                Tasks.Get(pTaskID).Recorder.Clear();
            }
        }

        /// <summary>
        /// Flags a job to run again before it's delay has finished.
        /// </summary>
        public void Resume()
        {
            _isSuspended = false;

            if (!_isRunning)
            {
                return;
            }

            _eventDelay.Set();
        }

        /// <summary>
        /// Flags the job to shutdown. It will stop execution
        /// as soon as possible.
        /// </summary>
        public void ShutDown()
        {
            if (!_isRunning)
            {
                // bug: Allow a job to be shutdown that hasn't yet been started (i.e. waiting forever on a failed parent job).
                if (_worker == null && State == eSTATE.NONE)
                {
                    Finished();
                }
                return;
            }

            _isRunning = false;
            _eventDelay.Set();
        }

        /// <summary>
        /// Starts the job.
        /// </summary>
        public void Start()
        {
            if (_worker != null)
            {
                throw new JobException("Worker thread is already started.");
            }

            _worker = new Thread(WorkerProc);
            _worker.Start();
            ThreadID = _worker.ManagedThreadId;

            FireEvents.Action(JobStart, ID);
        }

        /// <summary>
        /// Suspends a job from performing tasks.
        /// </summary>
        public void Suspend()
        {
            if (!_isRunning)
            {
                return;
            }

            _isSuspended = true;
            _eventDelay.Set();
        }

        /// <summary>
        /// Returns the name of this job.
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0}[{1}]", Name, Code);
        }
    }
}