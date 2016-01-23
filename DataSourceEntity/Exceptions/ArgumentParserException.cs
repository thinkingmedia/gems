using System;

namespace DataSourceEntity.Exceptions
{
    public class ArgumentParserException : DataSourceEntityException
    {
        /// <summary>
        /// String Format constructor
        /// </summary>
        public ArgumentParserException(string pMessage, params object[] pValues)
            : base(pMessage, pValues)
        {
        }

        /// <summary>
        /// Inner exception constructor
        /// </summary>
        public ArgumentParserException(string pMessage, Exception pInner)
            : base(pMessage, pInner)
        {
        }
    }
}