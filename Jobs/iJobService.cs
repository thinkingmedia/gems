using System;
using System.Collections.Generic;
using Jobs.Reports;

namespace Jobs
{
    /// <summary>
    /// The job service API.
    /// </summary>
    public interface iJobService : iJobEvents
    {
        /// <summary>
        /// Clears all errors for a job.
        /// </summary>
        void ClearEvents(Guid pJobID, Guid pTaskID);

        /// <summary>
        /// Tells a job to continue before it's delay has finished.
        /// </summary>
        /// <param name="pID">The unique ID of the job.</param>
        void Resume(Guid pID);

        /// <summary>
        /// Starts a job.
        /// </summary>
        void Start(iJob pJob);

        /// <summary>
        /// Removes a job from the manager.
        /// </summary>
        /// <param name="pID">The unique ID of the job.</param>
        void Stop(Guid pID);

        /// <summary>
        /// Removes all jobs from the manager.
        /// </summary>
        void StopAll();

        /// <summary>
        /// Suspends a job.
        /// </summary>
        /// <param name="pID"></param>
        void Suspend(Guid pID);

        /// <summary>
        /// Gets a report about a running job.
        /// </summary>
        /// <param name="pID">ID of the job.</param>
        /// <param name="pIncludeTasks"></param>
        /// <returns>The job reference, or Null if no longer managed.</returns>
        iJobReport getJobReport(Guid pID, bool pIncludeTasks);

        /// <summary>
        /// Access the events for a job.
        /// </summary>
        /// <param name="pID">ID of the job.</param>
        /// <returns>The job event reference, or Null if no longer managed.</returns>
        iJobEvents getJobEvents(Guid pID);

        /// <summary>
        /// Returns a list of Job IDs.
        /// </summary>
        /// <returns>The IDs of the Jobs</returns>
        IEnumerable<Guid> getJobIDs();

        /// <summary>
        /// How many jobs in the manager are still active.
        /// </summary>
        /// <returns></returns>
        int getRunning();
    }
}