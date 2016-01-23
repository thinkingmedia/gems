using System;

namespace Jobs.Tasks.Events
{
    public interface iEventFactory
    {
        /// <summary>
        /// Creates an event object.
        /// </summary>
        iEventObject Create(eEVENT_SEVERITY pSeverity, string pType, string pDesc, string pMessage);

        /// <summary>
        /// Creates an event for an exception.
        /// </summary>
        iEventObject Create(eEVENT_SEVERITY pSeverity, Exception pException);
    }
}