using System;
using System.Linq;
using System.Windows.Forms;
using Jobs;
using Node.Actions;
using StructureMap;

namespace Node.Controls.Jobs
{
    public class JobControl : UserControl
    {
        /// <summary>
        /// Used to handle UI actions.
        /// </summary>
        protected iActionService ActionService;

        /// <summary>
        /// For monitoring changes in selections.
        /// </summary>
        protected iActiveJobService ActiveJobService;

        /// <summary>
        /// True when setJobID has been called.
        /// </summary>
        protected bool Configured;

        /// <summary>
        /// The job this panel monitors.
        /// </summary>
        protected Guid JobID = Guid.Empty;

        /// <summary>
        /// The job service.
        /// </summary>
        protected iJobService JobService;

        /// <summary>
        /// Called when a job has been assigned.
        /// </summary>
        protected virtual void onJobAssigned()
        {
        }

        /// <summary>
        /// The Job ID for this control.
        /// </summary>
        public Guid getJobID()
        {
            return JobID;
        }

        /// <summary>
        /// Assigns a job to the panel.
        /// </summary>
        public void setJobID(Guid pJobID)
        {
            JobID = pJobID;

            ActionService = ObjectFactory.GetInstance<iActionService>();
            JobService = ObjectFactory.GetInstance<iJobService>();
            ActiveJobService = ObjectFactory.GetInstance<iActiveJobService>();

            foreach (JobControl jobControl in Controls.OfType<JobControl>())
            {
                jobControl.setJobID(pJobID);
            }

            Configured = true;

            onJobAssigned();
        }
    }
}