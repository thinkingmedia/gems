using System;
using Common.Exceptions;

namespace DataSources.Exceptions
{
    public class ModelException : CommonException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ModelException(string pMessage, params object[] pValues)
            : base(pMessage, pValues)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ModelException(string pMessage, Exception pInner)
            : base(pMessage, pInner)
        {
        }
    }
}
