using System;
using System.Threading;
using Jobs;
using Jobs.States;
using Jobs.Tasks;
using Jobs.Tasks.Events;

namespace JobsTest.Mock
{
    public class MockJob : iJob
    {
        public static iJob Create()
        {
            return new MockJob("Test", "Test", "Test", new TaskCollection(new EventRecorderFactory()));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MockJob(string pPlugin, string pName, string pCode, iTaskCollection pTasks)
        {
            ParentID = Guid.Empty;
            ID = Guid.NewGuid();
            MaxErrors = 10;

            Plugin = pPlugin;
            Name = pName;
            Code = pCode;

            State = eSTATE.NONE;
            Tasks = pTasks;
        }

        public event Action<Guid> JobCreated;
        public event Action<Guid> JobStart;
        public event Action<Guid> JobFinish;
        public event Action<Guid, Exception> JobError;

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
        /// A unique ID for the job.
        /// </summary>
        public Guid ID { get; private set; }

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
        /// The ID of the job that must finish before this job can start.
        /// </summary>
        public Guid ParentID { get; private set; }

        /// <summary>
        /// The current state of the job.
        /// </summary>
        public eSTATE State { get; private set; }

        /// <summary>
        /// The task factory used by this task.
        /// </summary>
        public iTaskCollection Tasks { get; private set; }

        /// <summary>
        /// The ID of the worker thread.
        /// </summary>
        public int ThreadID { get; private set; }

        /// <summary>
        /// This timestamp depends upon the current state of the job.
        /// </summary>
        public DateTime TimeStamp { get; private set; }

        /// <summary>
        /// Clears the event record for all tasks.
        /// </summary>
        public void Clear()
        {
        }

        /// <summary>
        /// Clears the event record for a single task.
        /// </summary>
        /// <param name="pTaskID">The task ID to clear.</param>
        public void Clear(Guid pTaskID)
        {
        }

        /// <summary>
        /// Flags a job to run again before it's delay has finished.
        /// </summary>
        public void Resume()
        {
            State = eSTATE.IDLE;
        }

        /// <summary>
        /// Flags the job to shutdown. It will stop execution
        /// as soon as possible.
        /// </summary>
        public void ShutDown()
        {
            State = eSTATE.FINISHED;
        }

        /// <summary>
        /// Starts the job.
        /// </summary>
        public void Start()
        {
            TimeStamp = DateTime.Now;
            ThreadID = Thread.CurrentThread.ManagedThreadId;
            State = eSTATE.IDLE;
            Count++;
        }

        /// <summary>
        /// Suspends a job from performing tasks.
        /// </summary>
        public void Suspend()
        {
            State = eSTATE.SUSPENDED;
        }
    }
}