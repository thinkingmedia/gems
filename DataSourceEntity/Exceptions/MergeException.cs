using System;

namespace DataSourceEntity.Exceptions
{
    public class MergeException : DataSourceEntityException
    {
        /// <summary>
        /// String Format constructor
        /// </summary>
        public MergeException(string pMessage, params object[] pValues)
            : base(pMessage, pValues)
        {
        }

        /// <summary>
        /// Inner exception constructor
        /// </summary>
        public MergeException(string pMessage, Exception pInner)
            : base(pMessage, pInner)
        {
        }
    }
}