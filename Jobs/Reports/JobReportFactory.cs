using System.Collections.Generic;
using System.Linq;
using Common.Events;
using Jobs.Tasks;

namespace Jobs.Reports
{
    /// <summary>
    /// A factory class that allows outside code to access details about
    /// the internal job object.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class JobReportFactory : iJobReportFactory
    {
        /// <summary>
        /// The job this factory creates reports on.
        /// </summary>
        private readonly iJob _job;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pJob">The job to report on.</param>
        public JobReportFactory(iJob pJob)
        {
            _job = pJob;
        }

        /// <summary>
        /// Creates a job info object.
        /// </summary>
        /// <param name="pIncludeTasks"></param>
        /// <returns>The info object</returns>
        public iJobReport Create(bool pIncludeTasks)
        {
            List<iTaskReport> taskReports = pIncludeTasks
                ? (from entry in _job.Tasks
                   let id = entry.ID
                   let recorder = entry.Recorder 
                    select new TaskReport(id, entry.Task.getName(),recorder))
                        .ToList<iTaskReport>()
                : null;

            return new JobReport
                   {
                       ID = _job.ID,
                       Plugin = _job.Plugin,
                       Name = _job.Name,
                       Code = _job.Code,
                       ThreadID = _job.ThreadID,
                       State = _job.State,
                       Errors = _job.Errors,
                       MaxErrors = _job.MaxErrors,
                       Count = _job.Count,
                       TimeStamp = _job.TimeStamp,
                       Tasks = taskReports
                   };
        }
    }
}