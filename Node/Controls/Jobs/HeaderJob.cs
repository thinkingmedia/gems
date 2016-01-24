using System;
using System.Globalization;
using System.Windows.Forms;
using Common.Events;
using GemsLogger;
using Jobs.Reports;
using Timer = System.Timers.Timer;

namespace Node.Controls.Jobs
{
    /// <summary>
    /// Displays details about a specific job.
    /// </summary>
    internal sealed partial class HeaderJob : JobControl
    {
        /// <summary>
        /// Different states of the control button.
        /// The one with the dropdown menu.
        /// </summary>
        private enum eCONTROL
        {
            STOP,
            RESUME,
            SUSPEND
        }

        /// <summary>
        /// Logging
        /// </summary>
        private readonly Logger _logger = Logger.Create(typeof (HeaderJob));

        /// <summary>
        /// The UI updater
        /// </summary>
        private readonly Timer _timer;

        /// <summary>
        /// What the current control state is.
        /// </summary>
        private eCONTROL _control;

        /// <summary>
        /// Updates the details.
        /// </summary>
        private void UpdateDetails()
        {
            if (!Configured)
            {
                return;
            }

            iJobReport report = JobService.getJobReport(JobID, false);
            if (report == null)
            {
                _logger.Error("JobInfo is null.");
                return;
            }

            txtName.Text = report.Name;
            txtCounter.Text = report.Count.ToString(CultureInfo.InvariantCulture);

            string menuKey = string.Format("menu{0}", _control);
            btnControl.Text = menu.Items[menuKey].Text;

            txtStatus.Text = JobInfoTheme.StateText(report);
            txtStatus.ForeColor = JobInfoTheme.ForeColor(JobInfoTheme.eTHEME_ELEMENT.STATE, report);
            txtStatus.BackColor = JobInfoTheme.BackColor(JobInfoTheme.eTHEME_ELEMENT.STATE, report);

            txtErrors.Text = JobInfoTheme.ErrorsText(report);
            txtErrors.ForeColor = JobInfoTheme.ForeColor(JobInfoTheme.eTHEME_ELEMENT.ERRORS, report);
            txtErrors.BackColor = JobInfoTheme.BackColor(JobInfoTheme.eTHEME_ELEMENT.ERRORS, report);

            txtTimer.Text = JobInfoTheme.TimerText(report);
            txtTimer.ForeColor = JobInfoTheme.ForeColor(JobInfoTheme.eTHEME_ELEMENT.TIMER, report);
            txtTimer.BackColor = JobInfoTheme.BackColor(JobInfoTheme.eTHEME_ELEMENT.TIMER, report);
        }

        /// <summary>
        /// Handles the click event for changing job state.
        /// </summary>
        private void onBtnControl(object pSender, EventArgs pEvent)
        {
            if (!Configured)
            {
                return;
            }

            switch (_control)
            {
                case eCONTROL.RESUME:
                    ActionService.Trigger("Jobs.Resume");
                    break;
                case eCONTROL.SUSPEND:
                    ActionService.Trigger("Jobs.Suspend");
                    break;
                case eCONTROL.STOP:
                    ActionService.Trigger("Jobs.Stop");
                    break;
            }

            UpdateDetails();
        }

        /// <summary>
        /// Menu item on the dropdown control button was clicked.
        /// </summary>
        private void onMenuClick(object pSender, EventArgs pEvent)
        {
            ToolStripMenuItem item = pSender as ToolStripMenuItem;
            if (item == null)
            {
                return;
            }
            eCONTROL state;
            if (!Enum.TryParse(item.Name.ToUpper().Replace("MENU", ""), out state))
            {
                return;
            }
            _control = state;
            UpdateDetails();
        }

        /// <summary>
        /// Called every second to update the display.
        /// </summary>
        private void onTimerUpdate(object pSender, EventArgs pEventArgs)
        {
            FireEvents.Invoke(this, UpdateDetails);
        }

        /// <summary>
        /// Tells the control if it's visible on the screen.
        /// </summary>
        private void onVisibleChanged(object pSender, EventArgs pEventArgs)
        {
            _timer.Enabled = Visible;
            if (Visible)
            {
                UpdateDetails();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HeaderJob()
        {
            InitializeComponent();

            _control = eCONTROL.RESUME;

            Dock = DockStyle.Fill;

            _timer = new Timer {Enabled = true, Interval = 1000};
            _timer.Elapsed += onTimerUpdate;
            _timer.Start();

            UpdateDetails();
        }

        /// <summary>
        /// Call this before the application exists.
        /// </summary>
        public void StopTimer()
        {
            _timer.Stop();
        }

        /// <summary>
        /// Called when a job has been assigned.
        /// </summary>
        protected override void onJobAssigned()
        {
        }
    }
}