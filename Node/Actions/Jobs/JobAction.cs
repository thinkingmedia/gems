using System;
using System.Collections.Generic;
using System.Linq;
using Common.Annotations;
using GemsLogger;
using Jobs;
using Jobs.Reports;
using StructureMap;

namespace Node.Actions.Jobs
{
    /// <summary>
    /// Base class for actions that can be executed on a single or all jobs.
    /// </summary>
    internal abstract class JobAction : Action
    {
        /// <summary>
        /// Logging
        /// </summary>
        private static readonly Logger _logger = Logger.Create(typeof (JobAction));

        /// <summary>
        /// Provides the GUID for the currently selected job in the UI.
        /// </summary>
        protected readonly iActiveJobService ActiveJobService;

        /// <summary>
        /// Holds a list of active jobs.
        /// </summary>
        protected readonly iJobService JobService;

        /// <summary>
        /// The base name of the task. The name will be appended with up to 3 types.
        /// </summary>
        private readonly string _name;

        /// <summary>
        /// Does this action perform on selected tasks.
        /// </summary>
        private readonly bool _supportTasks;

        /// <summary>
        /// Logs the action taking place
        /// </summary>
        private void ExecuteJob(Guid pJobID, Guid pTaskID, string pAction)
        {
            iJobReport report = JobService.getJobReport(pJobID, false);
            if (pTaskID == Guid.Empty)
            {
                _logger.Fine("Job \"{0}\" {1} by user.", report.Name, pAction);
                Execute(pJobID);
            }
            else
            {
                iTaskReport taskReport = report.Tasks.FirstOrDefault(pTask=>pTask.ID == pTaskID);
                _logger.Fine("Task \"{0}.{1}\" {2} by user.", report.Name, taskReport == null ? "NULL" : taskReport.Name,
                    pAction);
                Execute(pJobID, pTaskID);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected JobAction([NotNull] string pName,
                            bool pSupportTasks = false)
        {
            if (pName == null)
            {
                throw new ArgumentNullException("pName");
            }

            _name = pName;
            _supportTasks = pSupportTasks;

            ActiveJobService = ObjectFactory.GetInstance<iActiveJobService>();
            JobService = ObjectFactory.GetInstance<iJobService>();
        }

        /// <summary>
        /// Optional if the action doesn't work on tasks.
        /// </summary>
        protected virtual void Execute(Guid pJobID, Guid pTaskID)
        {
        }

        /// <summary>
        /// Triggers the action.
        /// </summary>
        /// <param name="pName"></param>
        protected override void Execute(string pName)
        {
            if (pName == _name + ".All")
            {
                foreach (Guid all in JobService.getJobIDs())
                {
                    ExecuteJob(all, Guid.Empty, pName);
                }
                return;
            }

            Guid jobID = ActiveJobService.getActiveJob();
            if (jobID == Guid.Empty)
            {
                return;
            }
            if (pName == _name)
            {
                ExecuteJob(jobID, Guid.Empty, pName);
                return;
            }

            Guid taskID = ActiveJobService.getActiveTask();
            if (taskID == Guid.Empty)
            {
                return;
            }
            ExecuteJob(jobID, taskID, pName);
        }

        /// <summary>
        /// All actions work on jobs.
        /// </summary>
        protected abstract void Execute(Guid pJobID);

        /// <summary>
        /// The unique name for this action.
        /// </summary>
        public override IEnumerable<string> getNames()
        {
            List<string> names = new List<string>
                                 {
                                     _name,
                                     _name + ".All"
                                 };
            if (_supportTasks)
            {
                names.Add(_name + ".Task");
            }
            return names;
        }
    }
}