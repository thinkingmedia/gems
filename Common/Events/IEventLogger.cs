using System;

namespace Common.Events
{
    /// <summary>
    /// Allows components to Log important events that trigger e-mail notifications.
    /// </summary>
    public interface iEventLogger
    {
        /// <summary>
        /// Will record the event for the next e-mail manifest.
        /// </summary>
        void Info(String pStr, params object[] pArgs);

        /// <summary>
        /// Will record the event for the next e-mail manifest.
        /// </summary>
        void Warning(String pStr, params object[] pArgs);

        /// <summary>
        /// Will record the event, and send an e-mail now.
        /// </summary>
        void Error(String pStr, params object[] pArgs);
    }
}
