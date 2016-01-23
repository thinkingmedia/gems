using System;
using Common.Exceptions;

namespace DataSources.Exceptions
{
    public class BehaviorException : CommonException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BehaviorException(string pMessage, params object[] pValues)
            : base(pMessage, pValues)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BehaviorException(string pMessage, Exception pInner)
            : base(pMessage, pInner)
        {
        }
    }
}
