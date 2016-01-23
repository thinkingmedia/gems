using System;
using System.Windows.Forms;
using Jobs.Reports;
using Jobs.States;
using Node.Properties;

namespace Node.Actions.Jobs
{
    internal class Stop : JobAction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Stop()
            : base("Jobs.Stop")
        {
        }

        /// <summary>
        /// Triggers the action.
        /// </summary>
        protected override void Execute(Guid pJobID)
        {
            iJobReport report = JobService.getJobReport(pJobID, false);

            if (report.State == eSTATE.FINISHED
                || report.State == eSTATE.FAILED)
            {
                return;
            }

            DialogResult result = MessageBox.Show(
                MainFrm.getMainApplication(),
                string.Format(Resources.STOP_JOB_MESSAGE, report.Name),
                Resources.STOP_JOB_TITLE,
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button3);

            if (result != DialogResult.Yes)
            {
                return;
            }

            JobService.Stop(pJobID);
        }
    }
}