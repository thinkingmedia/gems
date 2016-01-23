using System;
using Jobs.Tasks.Events;

namespace Jobs.Tasks
{
    /// <summary>
    /// Stores information about a running task for a job.
    /// </summary>
    [Obsolete]
    public interface iTaskRecorder
    {
        /// <summary>
        /// The event record for this task.
        /// </summary>
        iEventRecorder getEventRecorder();

        /// <summary>
        /// The ID of the task.
        /// </summary>
        Guid getID();

        /// <summary>
        /// The name of this task.
        /// </summary>
        string getName();
    }
}