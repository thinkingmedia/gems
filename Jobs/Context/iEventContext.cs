using System;
using Jobs.Tasks.Events;

namespace Jobs.Context
{
    public interface iEventContext
    {
        /// <summary>
        /// Records an event as an error in the event log.
        /// </summary>
        void RecordEvent(string pType, string pDesc, string pMessage);

        /// <summary>
        /// Records an exception in the current task as
        /// being handled, but was thrown by the executor
        /// of the task.
        /// </summary>
        /// <param name="pException">The exception object.</param>
        void RecordException(Exception pException);

        /// <summary>
        /// Assigns an exception recorder to the current context.
        /// </summary>
        void setEventRecorder(iEventRecorder pRecorder);
    }
}