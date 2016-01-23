using System;
using Jobs.Tasks.Events;
using StructureMap;

namespace Jobs.Tasks
{
    /// <summary>
    /// Holds information about the execution of a task. It's success
    /// and failure details.
    /// </summary>
    internal sealed class TaskRecorder : iTaskRecorder
    {
        /// <summary>
        /// The name for the task.
        /// </summary>
        private readonly string _name;

        /// <summary>
        /// The event recorder for this task.
        /// </summary>
        private readonly iEventRecorder _recorder;

        /// <summary>
        /// The ID of the task.
        /// </summary>
        private readonly Guid _taskID;

        /// <summary>
        /// Constructor
        /// </summary>
        public TaskRecorder(Guid pTaskID, string pName)
        {
            _taskID = pTaskID;
            _name = pName;

            _recorder = ObjectFactory.Container.GetInstance<iEventRecorder>();
        }

        /// <summary>
        /// The ID of the task.
        /// </summary>
        public string getName()
        {
            return _name;
        }

        /// <summary>
        /// The event record for this task.
        /// </summary>
        public iEventRecorder getEventRecorder()
        {
            return _recorder;
        }

        /// <summary>
        /// The name of this task.
        /// </summary>
        public Guid getID()
        {
            return _taskID;
        }
    }
}