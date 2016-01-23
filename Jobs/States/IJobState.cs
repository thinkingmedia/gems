using System;

namespace Jobs.States
{
    /// <summary>
    /// Defines the interface for rules that control the state of a job.
    /// </summary>
    public interface iJobState
    {
        /// <summary>
        /// Should the job execute tasks before the
        /// first delay period.
        /// </summary>
        /// <returns>True to run tasks first, False to delay job first.</returns>
        bool TasksFirst();

        /// <summary>
        /// Should report how long the worker thread can sleep
        /// before creating another task of tasks to be performed.
        /// </summary>
        TimeSpan Delay();

        /// <summary>
        /// Checks if the job is finished and can be shut down.
        /// </summary>
        bool isFinished();
    }
}