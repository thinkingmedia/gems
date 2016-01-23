using System;
using System.Collections.Generic;
using System.Linq;
using Jobs;
using Jobs.Reports;
using Jobs.States;

namespace JobsTest.Mock
{
    public class MockJobService : iJobService
    {
        private readonly Dictionary<Guid, iJob> _jobs;

        private readonly Dictionary<Guid, iJobReportFactory> _reports;

        /// <summary>
        /// Constructor
        /// </summary>
        public MockJobService(iJob pJob)
        {
            _jobs = new Dictionary<Guid, iJob> {{pJob.ID, pJob}};
            _reports = new Dictionary<Guid, iJobReportFactory> {{pJob.ID, new JobReportFactory(pJob)}};
        }

        public event Action<Guid> JobCreated;
        public event Action<Guid> JobStart;
        public event Action<Guid> JobFinish;
        public event Action<Guid, Exception> JobError;

        /// <summary>
        /// Clears all errors for a job.
        /// </summary>
        public void ClearEvents(Guid pJobID, Guid pTaskID)
        {
            if (pTaskID == Guid.Empty)
            {
                _jobs[pJobID].Clear();
                return;
            }
            _jobs[pJobID].Clear(pTaskID);
        }

        /// <summary>
        /// Tells a job to continue before it's delay has finished.
        /// </summary>
        /// <param name="pID">The unique ID of the job.</param>
        public void Resume(Guid pID)
        {
            _jobs[pID].Resume();
        }

        /// <summary>
        /// Starts a job.
        /// </summary>
        public void Start(iJob pJob)
        {
            pJob.Start();
        }

        /// <summary>
        /// Removes a job from the manager.
        /// </summary>
        /// <param name="pID">The unique ID of the job.</param>
        public void Stop(Guid pID)
        {
            _jobs[pID].Suspend();
        }

        /// <summary>
        /// Removes all jobs from the manager.
        /// </summary>
        public void StopAll()
        {
            _jobs.Values.ToList().ForEach(pJob=>pJob.ShutDown());
        }

        /// <summary>
        /// Suspends a job.
        /// </summary>
        /// <param name="pID"></param>
        public void Suspend(Guid pID)
        {
            _jobs[pID].Suspend();
        }

        /// <summary>
        /// Gets a report about a running job.
        /// </summary>
        /// <param name="pID">ID of the job.</param>
        /// <param name="pIncludeTasks"></param>
        /// <returns>The job reference, or Null if no longer managed.</returns>
        public iJobReport getJobReport(Guid pID, bool pIncludeTasks)
        {
            return _reports[pID].Create(pIncludeTasks);
        }

        /// <summary>
        /// Access the events for a job.
        /// </summary>
        /// <param name="pID">ID of the job.</param>
        /// <returns>The job event reference, or Null if no longer managed.</returns>
        public iJobEvents getJobEvents(Guid pID)
        {
            return _jobs[pID];
        }

        /// <summary>
        /// Returns a list of Job IDs.
        /// </summary>
        /// <returns>The IDs of the Jobs</returns>
        public IEnumerable<Guid> getJobIDs()
        {
            return from job in _jobs select job.Key;
        }

        /// <summary>
        /// How many jobs in the manager are still active.
        /// </summary>
        /// <returns></returns>
        public int getRunning()
        {
            return _jobs.Values.Count(pJob=>pJob.State != eSTATE.FINISHED && pJob.State != eSTATE.FAILED);
        }
    }
}