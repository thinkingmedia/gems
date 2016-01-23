using System;
using System.Collections.Generic;
using Jobs.Tasks.Events;

namespace Jobs.Tasks
{
    /// <summary>
    /// Used to create tasks.
    /// </summary>
    public interface iTaskCollection : IEnumerable<iTaskEntry>
    {
        /// <summary>
        /// Adds a task to the factory.
        /// </summary>
        void Add(iTask pTask);

        /// <summary>
        /// Check if the task is in the collection.
        /// </summary>
        bool ContainsTask(Guid pTaskID);

        /// <summary>
        /// Gets a task by it's ID.
        /// </summary>
        iTaskEntry Get(Guid pTaskID);
    }
}