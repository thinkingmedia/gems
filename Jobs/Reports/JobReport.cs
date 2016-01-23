using System;
using System.Collections.Generic;
using Jobs.States;

namespace Jobs.Reports
{
    /// <summary>
    /// These objects are passed as arguments to event listeners.
    /// </summary>
    internal sealed class JobReport : iJobReport
    {
        /// <summary>
        /// The code for the job.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The number of times the job has executed.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// The number of errors for this job.
        /// </summary>
        public int Errors { get; set; }

        /// <summary>
        /// The ID of the job.
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// Max allowed errors.
        /// </summary>
        public int MaxErrors { get; set; }

        /// <summary>
        /// The name of the plug-in that created the job.
        /// </summary>
        public string Plugin { get; set; }

        /// <summary>
        /// The name of the job.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The state of the job.
        /// </summary>
        public eSTATE State { get; set; }

        /// <summary>
        /// The reports for the tasks.
        /// </summary>
        public List<iTaskReport> Tasks { get; set; }

        /// <summary>
        /// The thread ID of the worker.
        /// </summary>
        public int ThreadID { get; set; }

        /// <summary>
        /// This timestamp depends upon the current state of the job.
        /// </summary>
        public DateTime TimeStamp { get; set; }

/*
        /// <summary>
        /// Shortens a job title to fit on a tab.
        /// </summary>
        private static string ShortenTitle(string pTitle, int pMaxLen = 18)
        {
            if (pTitle.Length > pMaxLen)
            {
                pTitle = pTitle.Substring(0, pMaxLen) + "..";
            }
            return pTitle;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}-{1}", ShortenTitle(Name), StateStr);
        }
*/
    }
}