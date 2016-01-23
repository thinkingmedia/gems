using System;
using Jobs.Tasks.Events;

namespace Jobs.Tasks
{
    public class TaskEntry : iTaskEntry
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TaskEntry(iTask pTask, iEventRecorder pEventRecorder)
        {
            if (pTask == null || pEventRecorder == null)
            {
                throw new NullReferenceException();
            }

            ID = Guid.NewGuid();
            Task = pTask;
            Recorder = pEventRecorder;
        }

        /// <summary>
        /// The unique ID for this entry.
        /// </summary>
        public Guid ID { get; private set; }

        /// <summary>
        /// The events for this task.
        /// </summary>
        public iEventRecorder Recorder { get; private set; }

        /// <summary>
        /// The task object.
        /// </summary>
        public iTask Task { get; private set; }
    }
}