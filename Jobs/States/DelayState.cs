using System;
using Jobs.Exceptions;

namespace Jobs.States
{
    /// <summary>
    /// A job state that is never ending with a fixed delay interval.
    /// </summary>
    public class DelayState : iJobState
    {
        /// <summary>
        /// The finished state.
        /// </summary>
        private bool _finished { get; set; }

        /// <summary>
        /// The delay interval.
        /// </summary>
        private TimeSpan _interval { get; set; }

        /// <summary>
        /// Assumes a format that contains [d.]hh:mm[:ss] (elements in brackets are optional)
        /// String can not be empty.
        /// </summary>
        private static TimeSpan CreateTimeSpan(string pStr)
        {
            if (string.IsNullOrWhiteSpace(pStr))
            {
                throw new DelayStateException("Time expression can not be blank or Null.");
            }

            try
            {
                return TimeSpan.Parse(pStr.Trim());
            }
            catch (Exception e)
            {
                throw new DelayStateException("Unable to parse time expression.", e);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DelayState(int pDays, int pHours, int pMinutes, int pSeconds)
            : this(new TimeSpan(pDays, pHours, pMinutes, pSeconds))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DelayState(string pExpression)
            : this(CreateTimeSpan(pExpression))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private DelayState(TimeSpan pDelay)
        {
            if (pDelay == null)
            {
                throw new DelayStateException("Time delay can not be null");
            }
            if (Math.Abs(pDelay.TotalSeconds) < double.Epsilon)
            {
                throw new DelayStateException("Time span can not be zero.");
            }
            _interval = pDelay;
            _finished = false;
        }

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
        /// The delay interval.
        /// </summary>
        public TimeSpan Delay()
        {
            return _interval;
        }

        /// <summary>
        /// The constant finished state (usually false).
        /// </summary>
        /// <returns></returns>
        public bool isFinished()
        {
            return _finished;
        }
    }
}