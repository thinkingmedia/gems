using System;

namespace Jobs.Exceptions
{
    public class StorageException : EngineException
    {
        /// <summary>
        /// String Format constructor
        /// </summary>
        public StorageException(string pMessage, params object[] pValues)
            : base(pMessage, pValues)
        {
        }

        /// <summary>
        /// Inner exception constructor
        /// </summary>
        public StorageException(string pMessage, Exception pInner)
            : base(pMessage, pInner)
        {
        }
    }
}