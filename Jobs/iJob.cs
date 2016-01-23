using System;
using Jobs.States;
using Jobs.Tasks;

namespace Jobs
{
    public interface iJob : iJobEvents
    {
        /// <summary>
        /// A unique code value that identifies this job.
        /// </summary>
        string Code { get; }

        /// <summary>
        /// The number of times this job has executed.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// How many unexpected exceptions this job has raised.
        /// </summary>
        int Errors { get; }

        /// <summary>
        /// A unique ID for the job.
        /// </summary>
        Guid ID { get; }

        /// <summary>
        /// The max number of unexpected errors before the job stops.
        /// Set to zero for unlimited.
        /// </summary>
        int MaxErrors { get; }

        /// <summary>
        /// The name of the plug-in that created the job.
        /// </summary>
        string Plugin { get; }

        /// <summary>
        /// The name of the job.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The ID of the job that must finish before this job can start.
        /// </summary>
        Guid ParentID { get; }

        /// <summary>
        /// The current state of the job.
        /// </summary>
        eSTATE State { get; }

        /// <summary>
        /// The task factory used by this task.
        /// </summary>
        iTaskCollection Tasks { get; }

        /// <summary>
        /// The ID of the worker thread.
        /// </summary>
        int ThreadID { get; }

        /// <summary>
        /// This timestamp depends upon the current state of the job.
        /// </summary>
        DateTime TimeStamp { get; }

        /// <summary>
        /// Clears the event record for all tasks.
        /// </summary>
        void Clear();

        /// <summary>
        /// Clears the event record for a single task.
        /// </summary>
        /// <param name="pTaskID">The task ID to clear.</param>
        void Clear(Guid pTaskID);

        /// <summary>
        /// Flags a job to run again before it's delay has finished.
        /// </summary>
        void Resume();

        /// <summary>
        /// Flags the job to shutdown. It will stop execution
        /// as soon as possible.
        /// </summary>
        void ShutDown();

        /// <summary>
        /// Starts the job.
        /// </summary>
        void Start();

        /// <summary>
        /// Suspends a job from performing tasks.
        /// </summary>
        void Suspend();
    }
}