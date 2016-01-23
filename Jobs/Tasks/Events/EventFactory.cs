using System;

namespace Jobs.Tasks.Events
{
    /// <summary>
    /// The event factory class.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EventFactory : iEventFactory
    {
        /// <summary>
        /// Creates an event object.
        /// </summary>
        public iEventObject Create(eEVENT_SEVERITY pSeverity, string pType, string pDesc, string pMessage)
        {
            return new EventObject(pSeverity, pType, pDesc, pMessage);
        }

        /// <summary>
        /// Creates an event for an exception.
        /// </summary>
        public iEventObject Create(eEVENT_SEVERITY pSeverity, Exception pException)
        {
            return new EventObject(pSeverity, pException);
        }
    }
}