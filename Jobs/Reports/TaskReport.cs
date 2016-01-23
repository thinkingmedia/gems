using System;
using Jobs.Tasks.Events;

namespace Jobs.Reports
{
    /// <summary>
    /// A read only report on a task.
    /// </summary>
    internal class TaskReport : iTaskReport
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TaskReport(Guid pTaskID, string pName, iEventRecorder pEventRecorder)
        {
            ID = pTaskID;
            Name = pName;
            EventRecorder = pEventRecorder;
        }

        /// <summary>
        /// The task ID.
        /// </summary>
        public Guid ID { get; private set; }

        /// <summary>
        /// The name of the task.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The even record.
        /// </summary>
        public iEventRecorder EventRecorder { get; private set; }
    }
}