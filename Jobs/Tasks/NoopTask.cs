using System.Threading;
using Jobs.Context;
using Jobs.Tasks.Events;

namespace Jobs.Tasks
{
    public class NoopTask : iTask
    {
        /// <summary>
        /// Called by the worker thread.
        /// </summary>
        public void Execute(JobContext pContext, iEventRecorder pEventRecorder)
        {
            Thread.Sleep(5000);
        }

        /// <summary>
        /// The name of this task
        /// </summary>
        public string getName()
        {
            return GetType().Name;
        }
    }
}