using System;
using System.Collections;
using System.Collections.Generic;
using Jobs.Tasks.Events;

namespace Jobs.Tasks
{
    /// <summary>
    /// PluginFactory objects create collections of tasks to be assigned to a job.
    /// </summary>
    public class TaskCollection : iTaskCollection
    {
        /// <summary>
        /// The factory used to create event recorders.
        /// </summary>
        private readonly iEventRecorderFactory _factory;

        /// <summary>
        /// The list of executable tasks.
        /// </summary>
        private readonly Dictionary<Guid, iTaskEntry> _tasks;

        /// <summary>
        /// Creates an empty task container.
        /// </summary>
        public TaskCollection(iEventRecorderFactory pFactory)
        {
            _factory = pFactory;
            _tasks = new Dictionary<Guid, iTaskEntry>();
        }

        /// <summary>
        /// Adds a Task to the collection.
        /// </summary>
        /// <param name="pTask">The task to add.</param>
        public void Add(iTask pTask)
        {
            lock (_tasks)
            {
                iTaskEntry entry = new TaskEntry(pTask, _factory.Create());
                _tasks.Add(entry.ID, entry);
            }
        }

        /// <summary>
        /// Check if the task is in the collection.
        /// </summary>
        public bool ContainsTask(Guid pTaskID)
        {
            lock (_tasks)
            {
                return _tasks.ContainsKey(pTaskID);
            }
        }

        /// <summary>
        /// Gets a task by it's ID.
        /// </summary>
        public iTaskEntry Get(Guid pTaskID)
        {
            lock (_tasks)
            {
                return _tasks[pTaskID];
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<iTaskEntry> GetEnumerator()
        {
            lock (_tasks)
            {
                return _tasks.Values.GetEnumerator();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}