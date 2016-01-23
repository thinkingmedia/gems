using System;
using Common.Exceptions;

namespace Jobs.Exceptions
{
    /// <summary>
    /// The base exception for all exceptions in this library.
    /// </summary>
    public class EngineException : CommonException
    {
        /// <summary>
        /// String Format constructor
        /// </summary>
        public EngineException(string pMessage, params object[] pValues)
            : base(pMessage, pValues)
        {
        }

        /// <summary>
        /// Inner exception constructor
        /// </summary>
        public EngineException(string pMessage, Exception pInner)
            : base(pMessage, pInner)
        {
        }
    }
}