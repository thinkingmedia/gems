using System;

namespace Jobs.States
{
    /// <summary>
    /// A job state that executes tasks only once.
    /// </summary>
    public class RunOnceState : iJobState
    {
        /// <summary>
        /// The inner state
        /// </summary>
        private bool _finished;

        /// <summary>
        /// Should the job execute tasks before the
        /// first delay period.
        /// </summary>
        /// <returns>True to run tasks first, False to delay job first.</returns>
        public bool TasksFirst()
        {
            return true;
        }

        /// <summary>
        /// There is no delay.
        /// </summary>
        public TimeSpan Delay()
        {
            _finished = true;
            return new TimeSpan(0);
        }

        /// <summary>
        /// It's always finished.
        /// </summary>
        public bool isFinished()
        {
            return _finished;
        }
    }
}