using System;

namespace Jobs.States
{
    /// <summary>
    /// Designed to run a job once per day. Even if the node is restarted
    /// within that day.
    /// </summary>
    public class DailyState : iJobState
    {
        /// <summary>
        /// The hour to run the job.
        /// </summary>
        private readonly int _hour;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pHour">Hour in 24 hour time to run the job.</param>
        public DailyState(int pHour)
        {
            _hour = pHour;
        }

        /// <summary>
        /// Should the job execute tasks before the
        /// first delay period.
        /// </summary>
        /// <returns>True to run tasks first, False to delay job first.</returns>
        public bool TasksFirst()
        {
            return false;
        }

        /// <summary>
        /// Always delays until the next day.
        /// </summary>
        public TimeSpan Delay()
        {
            DateTime when = Convert.ToDateTime(string.Format("{0}:00", _hour));
            DateTime next = when > DateTime.Now ? when : when.AddDays(1);
            return next - DateTime.Now;
        }

        /// <summary>
        /// Checks if the job is finished and can be shut down.
        /// </summary>
        public bool isFinished()
        {
            return false;
        }
    }
}