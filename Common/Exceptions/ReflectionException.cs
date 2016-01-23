using System;

namespace Common.Exceptions
{
    /// <summary>
    /// Used to report reflection problems.
    /// </summary>
    public class ReflectionException : CommonException
    {
        /// <summary>
        /// String Format constructor
        /// </summary>
        public ReflectionException(string pMessage, params object[] pValues)
            : base(pMessage, pValues)
        {
        }

        /// <summary>
        /// Inner exception constructor
        /// </summary>
        public ReflectionException(string pMessage, Exception pInner)
            : base(pMessage, pInner)
        {
        }
    }
}
