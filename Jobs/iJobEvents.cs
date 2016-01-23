using System;

namespace Jobs
{
    /// <summary>
    /// The events for a job.
    /// </summary>
    public interface iJobEvents
    {
        /// <summary>
        /// Called when a job is added to the manger.
        /// </summary>
        event Action<Guid> JobCreated;

        /// <summary>
        /// Called when the worker thread is started.
        /// </summary>
        event Action<Guid> JobStart;

        /// <summary>
        /// Called when the worker thread exists.
        /// </summary>
        event Action<Guid> JobFinish;

        /// <summary>
        /// Called if the tasks raise an exception.
        /// </summary>
        event Action<Guid, Exception> JobError;
    }
}