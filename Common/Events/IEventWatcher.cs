using System;
using System.Collections.Generic;

namespace Common.Events
{
    /// <summary>
    /// Allows the EventWatcher component to display the history of events.
    /// </summary>
    public interface iEventWatcher
    {
        /// <summary>
        /// Returns a list of event entries as DataCommand records.
        /// </summary>
        IEnumerable<EventLogger.EventRecord> getEvents(int pAmount);

        /// <summary>
        /// Clears the event Log.
        /// </summary>
        bool clear();

        /// <summary>
        /// Triggered when a new has been logged.
        /// </summary>
        event EventHandler onNewEvent;

        /// <summary>
        /// Tells if this watcher can perform tests of the e-mailer.
        /// </summary>
        /// <returns></returns>
        bool canTestEMail();

        /// <summary>
        /// Returns true if e-mail sent successfully.
        /// </summary>
        bool sendTestEMail();
    }
}
