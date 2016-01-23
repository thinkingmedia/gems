using Jobs.Context;
using Jobs.Tasks.Events;

namespace Jobs.Tasks
{
    public interface iTask
    {
        /// <summary>
        /// Called by the worker thread.
        /// </summary>
        void Execute(JobContext pContext, iEventRecorder pEventRecorder);

        /// <summary>
        /// The name of this task
        /// </summary>
        string getName();
    }
}