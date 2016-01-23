using System;
using Common.Annotations;

namespace Jobs.States
{
    /// <summary>
    /// Will only run the child state if the current time is between a range.
    /// </summary>
    public class TimeRangeState : iJobState
    {
        /// <summary>
        /// The child state.
        /// </summary>
        private readonly iJobState _child;

        /// <summary>
        /// The end time in ToDateTime format for time only.
        /// </summary>
        private readonly string _endTime;

        /// <summary>
        /// The start time in ToDateTime format for time only.
        /// </summary>
        private readonly string _startTime;

        /// <summary>
        /// The end time
        /// </summary>
        private DateTime getEnd()
        {
            return Convert.ToDateTime(_endTime);
        }

        /// <summary>
        /// The start time.
        /// </summary>
        private DateTime getStart()
        {
            return Convert.ToDateTime(_startTime);
        }

        /// <summary>
        /// Is the current time inside the active range.
        /// </summary>
        private bool isActive()
        {
            DateTime now = DateTime.Now;
            return now > getStart() && now < getEnd();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pChild">The child state.</param>
        /// <param name="pStartTime">The start time in HH:MM:SS format.</param>
        /// <param name="pEndTime">The end time in HH:MM:SS format.</param>
        public TimeRangeState([NotNull] iJobState pChild, [NotNull] string pStartTime, [NotNull] string pEndTime)
        {
            if (pChild == null)
            {
                throw new ArgumentNullException("pChild");
            }
            if (pStartTime == null)
            {
                throw new ArgumentNullException("pStartTime");
            }
            if (pEndTime == null)
            {
                throw new ArgumentNullException("pEndTime");
            }

            _child = pChild;
            _startTime = pStartTime;
            _endTime = pEndTime;
        }

        /// <summary>
        /// Should the job execute tasks before the
        /// first delay period.
        /// </summary>
        /// <returns>True to run tasks first, False to delay job first.</returns>
        public bool TasksFirst()
        {
            return isActive() && _child.TasksFirst();
        }

        /// <summary>
        /// Should report how long the worker thread can sleep
        /// before creating another task of tasks to be performed.
        /// </summary>
        public TimeSpan Delay()
        {
            if (isActive())
            {
                return _child.Delay();
            }

            DateTime start = getStart();
            if (start < DateTime.Now)
            {
                start = start.AddDays(1);
            }
            return start - DateTime.Now;
        }

        /// <summary>
        /// Checks if the job is finished and can be shut down.
        /// </summary>
        public bool isFinished()
        {
            return _child.isFinished();
        }
    }
}