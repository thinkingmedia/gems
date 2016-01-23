using System;
using System.Collections.Generic;
using Jobs.States;

namespace Jobs.Reports
{
    public interface iJobReport
    {
        /// <summary>
        /// The code for the job.
        /// </summary>
        string Code { get; }

        /// <summary>
        /// The number of times the job has executed.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// The number of errors for this job.
        /// </summary>
        int Errors { get; }

        /// <summary>
        /// The ID of the job.
        /// </summary>
        Guid ID { get; }

        /// <summary>
        /// Max allowed errors.
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
        /// The state of the job.
        /// </summary>
        eSTATE State { get; }

        /// <summary>
        /// The reports for the tasks.
        /// </summary>
        List<iTaskReport> Tasks { get; }

        /// <summary>
        /// The thread ID of the worker.
        /// </summary>
        int ThreadID { get; }

        /// <summary>
        /// This timestamp depends upon the current state of the job.
        /// </summary>
        DateTime TimeStamp { get; }
    }
}