using System;
using System.Windows.Forms;
using Common.Events;
using Jobs;
using Node.Controls.Jobs;

namespace Node.Controls
{
    /// <summary>
    /// Handles the displaying of individual jobs using tabs to
    /// select the job.
    /// </summary>
    internal partial class JobsViewStack : UserControl
    {
        /// <summary>
        /// Makes the panel for a job ID active.
        /// </summary>
        /// <param name="pID">The job ID.</param>
        private void Activate(Guid pID)
        {
            string id = pID.ToString();
            if (tabs.TabPages.ContainsKey(id))
            {
                tabs.SelectTab(tabs.TabPages[id]);
            }
        }

        /// <summary>
        /// Adds a logger to the UI. Giving that log it's own tab.
        /// </summary>
        private void Add(PanelJob pPanel)
        {
            string id = pPanel.getJobID().ToString();
            tabs.TabPages.Add(id, pPanel.getTitle());
            tabs.TabPages[id].Controls.Add(pPanel);
        }

        /// <summary>
        /// Adds a JobPanel for each job created.
        /// </summary>
        private void onJobCreated(Guid pJobID)
        {
            FireEvents.Invoke(this, ()=>
                                    {
                                        PanelJob panelJob = new PanelJob(pJobID);
                                        Add(panelJob);
                                    });
        }

        /// <summary>
        /// When a tab is selected.
        /// </summary>
        private void onTabSelectionChanged(object pSender, TabControlEventArgs pEventArgs)
        {
            foreach (TabPage tabPage in tabs.TabPages)
            {
                tabPage.Controls[0].Visible = tabPage.TabIndex == tabs.SelectedTab.TabIndex;
            }
        }

        /// <summary>
        /// Handles updating the UI elements.
        /// </summary>
        private void onTimer(object pSender, EventArgs pEventArgs)
        {
            foreach (TabPage page in tabs.TabPages)
            {
                PanelJob panel = (PanelJob)page.Controls[0];
                string title = panel.getTitle();
                if (page.Text != title)
                {
                    page.Text = panel.getTitle();
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public JobsViewStack(iActiveJobService pActiveJobService, iJobService pJobService)
        {
            InitializeComponent();

            // add a tab for the main thread
            Add(new PanelJob(Guid.Empty));

            pJobService.JobCreated += onJobCreated;
            pActiveJobService.JobChanged += Activate;
        }

        /// <summary>
        /// Should the control update the name of tabs
        /// constantly
        /// </summary>
        /// <param name="pActive">True to update tab names.</param>
        public void setUpdateTabs(bool pActive)
        {
            _timer.Enabled = pActive;
        }
    }
}