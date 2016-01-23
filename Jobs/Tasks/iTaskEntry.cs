using System;
using Jobs.Tasks.Events;

namespace Jobs.Tasks
{
    /// <summary>
    /// Represents the storage of a task in the task collection.
    /// </summary>
    public interface iTaskEntry
    {
        /// <summary>
        /// The unique ID for this entry.
        /// </summary>
        Guid ID { get; }

        /// <summary>
        /// The events for this task.
        /// </summary>
        iEventRecorder Recorder { get; }

        /// <summary>
        /// The task object.
        /// </summary>
        iTask Task { get; }
    }
}