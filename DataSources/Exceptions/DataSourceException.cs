using System;
using Common.Exceptions;

namespace DataSources.Exceptions
{
    public class DataSourceException : CommonException
    {
        /// <summary>
        /// String Format constructor
        /// </summary>
        public DataSourceException(string pMessage, params object[] pValues) : base(pMessage, pValues)
        {
        }

        /// <summary>
        /// Inner exception constructor
        /// </summary>
        public DataSourceException(string pMessage, Exception pInner) : base(pMessage, pInner)
        {
        }
    }
}