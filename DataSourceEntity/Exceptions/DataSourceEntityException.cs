using System;
using Common.Exceptions;

namespace DataSourceEntity.Exceptions
{
    public class DataSourceEntityException : CommonException
    {
        /// <summary>
        /// String Format constructor
        /// </summary>
        public DataSourceEntityException(string pMessage, params object[] pValues)
            : base(pMessage, pValues)
        {
        }

        /// <summary>
        /// Inner exception constructor
        /// </summary>
        public DataSourceEntityException(string pMessage, Exception pInner)
            : base(pMessage, pInner)
        {
        }
    }
}