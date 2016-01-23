using System;

namespace Common.Exceptions
{
    /// <summary>
    /// The base exception for all exceptions in this library.
    /// </summary>
    public class CommonException : Exception
    {
        /// <summary>
        /// String Format constructor
        /// </summary>
        public CommonException(string pMessage, params object[] pValues)
            : base(string.Format(pMessage, pValues))
        {
        }

        /// <summary>
        /// Inner exception constructor
        /// </summary>
        public CommonException(string pMessage, Exception pInner)
            : base(pMessage, pInner)
        {
        }
    }
}
