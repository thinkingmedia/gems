using System;
using System.Collections.Generic;
using System.Linq;
using Common.Events;
using Jobs.Reports;
using Jobs.States;
using Logging;
using StructureMap;

namespace Jobs
{
    /// <summary>
    /// Handles the execution of jobs.
    /// </summary>
    internal sealed class JobService : iJobService
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly Logger _logger = Logger.Create(typeof (JobService));

        /// <summary>
        /// List of report factories for jobs.
        /// </summary>
        private readonly Dictionary<Guid, iJobReportFactory> _jobReportFactories;

        /// <summary>
        /// List of jobs.
        /// </summary>
        private readonly Dictionary<Guid, iJob> _jobs;

        /// <summary>
        /// Callback from the executing job that an unexpected exception was thrown.
        /// </summary>
        private void onJobError(Guid pJobID, Exception pError)
        {
            FireEvents.Action(JobError, pJobID, pError);
        }

        /// <summary>
        /// Remove the job from the list when it's finished.
        /// </summary>
        private void onJobFinish(Guid pJobID)
        {
            _logger.Fine("Job Finished: {0}", pJobID);

            FireEvents.Action(JobFinish, pJobID);

            iJobReport report = getJobReport(pJobID, false);
            if (report == null || report.State == eSTATE.FAILED)
            {
                return;
            }

            lock (_jobs)
            {
                (from job in _jobs.Values
                 where job.ParentID == pJobID
                       && job.State == eSTATE.NONE
                 select job)
                    .ToList()
                    .ForEach(pJob=>pJob.Start());
            }
        }

        /// <summary>
        /// Informs listeners that the job started.
        /// </summary>
        private void onJobStart(Guid pJobID)
        {
            _logger.Fine("Job Start: {0}", pJobID);
            FireEvents.Action(JobStart, pJobID);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public JobService()
        {
            _jobs = new Dictionary<Guid, iJob>();
            _jobReportFactories = new Dictionary<Guid, iJobReportFactory>();
        }

        /// <summary>
        /// Called when a job is started.
        /// </summary>
        public event Action<Guid> JobStart;

        /// <summary>
        /// Called when a job is finished.
        /// </summary>
        public event Action<Guid> JobFinish;

        /// <summary>
        /// Called when a job throws an exception.
        /// </summary>
        public event Action<Guid, Exception> JobError;

        /// <summary>
        /// Returns a list of Job IDs.
        /// </summary>
        /// <returns>The IDs of the Jobs</returns>
        public IEnumerable<Guid> getJobIDs()
        {
            lock (_jobs)
            {
                return new List<Guid>(_jobs.Keys);
            }
        }

        /// <summary>
        /// Called when a job is added to the manger.
        /// </summary>
        public event Action<Guid> JobCreated;

        /// <summary>
        /// Access a job.
        /// </summary>
        /// <param name="pID">ID of the job.</param>
        /// <param name="pIncludeTasks"></param>
        /// <returns>The job reference, or Null if no longer managed.</returns>
        public iJobReport getJobReport(Guid pID, bool pIncludeTasks)
        {
            lock (_jobReportFactories)
            {
                return _jobReportFactories.ContainsKey(pID)
                    ? _jobReportFactories[pID].Create(pIncludeTasks)
                    : null;
            }
        }

        /// <summary>
        /// Access the events for a job.
        /// </summary>
        /// <param name="pID">ID of the job.</param>
        /// <returns>The job event reference, or Null if no longer managed.</returns>
        public iJobEvents getJobEvents(Guid pID)
        {
            lock (_jobs)
            {
                return _jobs.ContainsKey(pID) ? _jobs[pID] : null;
            }
        }

        /// <summary>
        /// Suspends a job.
        /// </summary>
        /// <param name="pID"></param>
        public void Suspend(Guid pID)
        {
            lock (_jobs)
            {
                if (_jobs.ContainsKey(pID))
                {
                    _jobs[pID].Suspend();
                }
            }
        }

        /// <summary>
        /// Removes a job from the manager.
        /// </summary>
        /// <param name="pID">The unique name of the job.</param>
        public void Stop(Guid pID)
        {
            lock (_jobs)
            {
                if (_jobs.ContainsKey(pID))
                {
                    _jobs[pID].ShutDown();
                }
            }
        }

        /// <summary>
        /// Clears all events for a job.
        /// </summary>
        public void ClearEvents(Guid pJobID, Guid pTaskID)
        {
            lock (_jobs)
            {
                if (!_jobs.ContainsKey(pJobID))
                {
                    return;
                }

                if (pTaskID == Guid.Empty)
                {
                    _jobs[pJobID].Clear();
                    return;
                }

                _jobs[pJobID].Clear(pTaskID);
            }
        }

        /// <summary>
        /// How many jobs in the manager are still active.
        /// </summary>
        /// <returns></returns>
        public int getRunning()
        {
            lock (_jobs)
            {
                return _jobs.Count(pJob=>pJob.Value.State != eSTATE.FINISHED && pJob.Value.State != eSTATE.FAILED);
            }
        }

        /// <summary>
        /// Tells a job to continue before it's delay has finished.
        /// </summary>
        /// <param name="pID">The unique ID of the job.</param>
        public void Resume(Guid pID)
        {
            lock (_jobs)
            {
                if (_jobs.ContainsKey(pID))
                {
                    _jobs[pID].Resume();
                }
            }
        }

        /// <summary>
        /// Removes all jobs from the manager.
        /// </summary>
        public void StopAll()
        {
            getJobIDs().ToList().ForEach(Stop);
        }

        /// <summary>
        /// Adds a job to the manager.
        /// </summary>
        /// <param name="pJob">The job object.</param>
        public void Start(iJob pJob)
        {
            pJob.JobFinish += onJobFinish;
            pJob.JobStart += onJobStart;
            pJob.JobError += onJobError;

            lock (_jobs)
            {
                _jobs.Add(pJob.ID, pJob);
            }

            lock (_jobReportFactories)
            {
                iJobReportFactory reportFactory = ObjectFactory.Container.With(pJob).GetInstance<iJobReportFactory>();
                _jobReportFactories.Add(pJob.ID, reportFactory);
            }

            _logger.Fine("Job Added: {0}", pJob);

            FireEvents.Action(JobCreated, pJob.ID);

            lock (_jobs)
            {
                if (pJob.ParentID == Guid.Empty)
                {
                    _jobs[pJob.ID].Start();
                }
            }
        }

        /// <summary>
        /// How many jobs are in the manager.
        /// </summary>
        public int getCount()
        {
            lock (_jobs)
            {
                return _jobs.Count;
            }
        }
    }
}