using System;
using Jobs.Tasks.Events;

namespace Jobs.Reports
{
    /// <summary>
    /// A description of a task taken as a snapshot.
    /// </summary>
    public interface iTaskReport
    {
        /// <summary>
        /// The history of events for this task.
        /// </summary>
        iEventRecorder EventRecorder { get; }

        /// <summary>
        /// The task ID.
        /// </summary>
        Guid ID { get; }

        /// <summary>
        /// The name of the task.
        /// </summary>
        string Name { get; }
    }
}