using System.Collections.Generic;

namespace Jobs.Tasks.Events
{
    public interface iEventRecorder
    {
        /// <summary>
        /// Records an event.
        /// </summary>
        void Add(iEventObject pEvent);

        /// <summary>
        /// Clears the event recorder history.
        /// </summary>
        void Clear();

        /// <summary>
        /// Counts how many events in total has occurred.
        /// </summary>
        int getCount();

        /// <summary>
        /// Counts how many events in total have occurred by type.
        /// </summary>
        int getCount(eEVENT_SEVERITY pSeverity);

        /// <summary>
        /// Gets a history of past events.
        /// </summary>
        List<iEventObject> getEvents();
    }
}