using System;

namespace Jobs.Exceptions
{
    public class AbortJobException : JobException
    {
        /// <summary>
        /// String Format constructor
        /// </summary>
        public AbortJobException(string pMessage, params object[] pValues)
            : base(pMessage, pValues)
        {
        }

        /// <summary>
        /// Inner exception constructor
        /// </summary>
        public AbortJobException(string pMessage, Exception pInner)
            : base(pMessage, pInner)
        {
        }
    }
}