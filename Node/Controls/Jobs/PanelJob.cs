using System;
using System.Threading;
using System.Windows.Forms;
using Common.Events;
using GemsLogger;
using GemsLogger.Formatters;
using Jobs;
using Jobs.Reports;
using Jobs.States;

namespace Node.Controls.Jobs
{
    /// <summary>
    /// UI component that display the active log of a running job.
    /// </summary>
    internal sealed partial class PanelJob : JobControl
    {
        /// <summary>
        /// Logging
        /// </summary>
        private static readonly Logger _logger = Logger.Create(typeof (PanelJob));

        /// <summary>
        /// Sets up the logger to follow a jobs thread.
        /// </summary>
        private void ConfigLogger(iJobReport pReport)
        {
            logger.threadID = pReport.ThreadID;
            DetailFormat.Register(pReport.ThreadID, pReport.Code);
        }

        /// <summary>
        /// Stops the logger from listening to the job.
        /// </summary>
        private void Destroy()
        {
            _headerJob.StopTimer();
            DetailFormat.Unregister(logger.threadID);
            logger.threadID = 0;
            logger.onStop();
        }

        /// <summary>
        /// Stop the logger when the job finishes.
        /// </summary>
        private void JobFinish(Guid pJobID)
        {
            FireEvents.Invoke(this, Destroy);
        }

        /// <summary>
        /// For panels that are created before a job is started.
        /// </summary>
        private void JobStart(Guid pJobID)
        {
            iJobReport report = JobService.getJobReport(JobID, false);
            if (report == null)
            {
                return;
            }
            ConfigLogger(report);
        }

        /// <summary>
        /// Enable panel when made visible.
        /// </summary>
        private void onVisibleChanged(object pSender, EventArgs pEventArgs)
        {
            if (!Configured || JobID == Guid.Empty)
            {
                return;
            }
            _headerJob.Visible = Visible;
            _taskPanel.Visible = Visible;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PanelJob(Guid pID)
        {
            InitializeComponent();

            Dock = DockStyle.Fill;

            _logger.Fine("JobPanel: {0}", pID);

            setJobID(pID);

            if (pID != Guid.Empty)
            {
                _headerJob.setJobID(pID);
                _taskPanel.setJobID(pID);
            }
            else
            {
                _tabs.Controls.Remove(_tabTask);
            }

            logger.onStart();

            if (JobID == Guid.Empty)
            {
                logger.threadID = Thread.CurrentThread.ManagedThreadId;
                headerPanel.Hide();
                return;
            }

            // sometimes a job crashes right when it starts and by the time
            // the code gets to here it's gone from the engine already
            iJobReport report = JobService.getJobReport(JobID, false);
            if (report == null)
            {
                return;
            }

            if (report.State != eSTATE.NONE)
            {
                ConfigLogger(report);
            }

            iJobEvents jobEvents = JobService.getJobEvents(JobID);
            jobEvents.JobStart += JobStart;
            jobEvents.JobFinish += JobFinish;
        }

        /// <summary>
        /// Called when a job has been assigned.
        /// </summary>
        protected override void onJobAssigned()
        {
        }

        /// <summary>
        /// The text to show on the tab.
        /// </summary>
        /// <returns>The title for the tab.</returns>
        public string getTitle()
        {
            if (JobID == Guid.Empty)
            {
                return "Main";
            }
            iJobReport report = JobService.getJobReport(JobID, false);
            return report == null ? "undefined" : report.ToString();
        }
    }
}