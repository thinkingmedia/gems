using System;

namespace Node.Controls.Jobs.Tasks
{
    public partial class PanelTask : JobControl
    {
        /// <summary>
        /// When the panel is made visible.
        /// </summary>
        private void onVisibleChanged(object pSender, EventArgs pEventArgs)
        {
            _taskDetail.Visible = Visible;
            _taskGridView.Visible = Visible;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PanelTask()
        {
            InitializeComponent();

            _taskDetail.EventObjects += pList=>_taskGridView.setEventObjects(pList);
        }

        /// <summary>
        /// Called when a job has been assigned.
        /// </summary>
        protected override void onJobAssigned()
        {
        }
    }
}