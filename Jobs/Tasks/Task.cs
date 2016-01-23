using Jobs.Context;
using Jobs.Tasks.Events;

namespace Jobs.Tasks
{
    /// <summary>
    /// Creates a Job that runs in a background thread.
    /// </summary>
    public abstract class Task : iTask
    {
        /// <summary>
        /// The name of this task
        /// </summary>
        private readonly string _name;

        /// <summary>
        /// Constructor
        /// </summary>
        protected Task()
        {
            _name = GetType().Name;
        }

        /// <summary>
        /// The name of this task
        /// </summary>
        public string getName()
        {
            return _name;
        }

        /// <summary>
        /// Called by the worker thread.
        /// </summary>
        public abstract void Execute(JobContext pContext, iEventRecorder pEventRecorder);
    }
}