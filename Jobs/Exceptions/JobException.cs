using System;

namespace Jobs.Exceptions
{
    /// <summary>
    /// Exceptions related to the management of a job.
    /// </summary>
    public class JobException : EngineException
    {
        /// <summary>
        /// String Format constructor
        /// </summary>
        public JobException(string pMessage, params object[] pValues)
            : base(pMessage, pValues)
        {
        }

        /// <summary>
        /// Inner exception constructor
        /// </summary>
        public JobException(string pMessage, Exception pInner)
            : base(pMessage, pInner)
        {
        }
    }
}