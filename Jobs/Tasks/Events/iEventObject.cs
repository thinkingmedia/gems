using System;

namespace Jobs.Tasks.Events
{
    public interface iEventObject
    {
        /// <summary>
        /// A unique ID for this event.
        /// </summary>
        Guid ID { get; }

        /// <summary>
        /// How severe
        /// </summary>
        eEVENT_SEVERITY Severity { get; }

        /// <summary>
        /// The type of event.
        /// </summary>
        string Type { get; }

        /// <summary>
        /// The short description.
        /// </summary>
        string Desc { get; }

        /// <summary>
        /// The event message.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// When it happen.
        /// </summary>
        DateTime When { get; }
    }
}