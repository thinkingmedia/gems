using System;

namespace Jobs.Tasks.Events
{
    /// <summary>
    /// Holds an exception and when it occurred.
    /// </summary>
    internal sealed class EventObject : iEventObject
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pSeverity">Severity of the error.</param>
        /// <param name="pType">The type of error.</param>
        /// <param name="pDesc">The description</param>
        /// <param name="pMessage">The lengthy message.</param>
        public EventObject(eEVENT_SEVERITY pSeverity, string pType, string pDesc, string pMessage)
        {
            if (pType == null)
            {
                throw new ArgumentNullException("pType");
            }

            if (pDesc == null)
            {
                throw new ArgumentNullException("pDesc");
            }

            if (pMessage == null)
            {
                throw new ArgumentNullException("pMessage");
            }

            ID = Guid.NewGuid();
            Severity = pSeverity;
            When = DateTime.Now;
            Type = pType;
            Desc = pDesc;
            Message = pMessage;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pSeverity">Severity of the error.</param>
        /// <param name="pWhat">The error to remember.</param>
        public EventObject(eEVENT_SEVERITY pSeverity, Exception pWhat)
            : this(pSeverity, pWhat.GetType().Name, pWhat.Message, pWhat.StackTrace ?? pWhat.Message)
        {
        }

        /// <summary>
        /// A unique ID for this event.
        /// </summary>
        public Guid ID { get; private set; }

        /// <summary>
        /// How severe
        /// </summary>
        public eEVENT_SEVERITY Severity { get; private set; }

        /// <summary>
        /// The type of event.
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// The short description.
        /// </summary>
        public string Desc { get; private set; }

        /// <summary>
        /// The event message.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// When it happen.
        /// </summary>
        public DateTime When { get; private set; }
    }
}