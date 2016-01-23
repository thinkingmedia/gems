using System;

namespace Jobs.Exceptions
{
    /// <summary>
    /// Exceptions related the timing of a job.
    /// </summary>
    public class DelayStateException : EngineException
    {
        /// <summary>
        /// String Format constructor
        /// </summary>
        public DelayStateException(string pMessage, params object[] pValues)
            : base(pMessage, pValues)
        {
        }

        /// <summary>
        /// Inner exception constructor
        /// </summary>
        public DelayStateException(string pMessage, Exception pInner)
            : base(pMessage, pInner)
        {
        }
    }
}