using GemsLoggerUI;
using Node.Controls.Jobs.Tasks;

namespace Node.Controls.Jobs
{
    sealed partial class PanelJob
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.logger = new GemsLoggerUI.LoggerControl();
            this.headerPanel = new System.Windows.Forms.Panel();
            this._headerJob = new Node.Controls.Jobs.HeaderJob();
            this._tabs = new System.Windows.Forms.TabControl();
            this._tabJob = new System.Windows.Forms.TabPage();
            this._tabTask = new System.Windows.Forms.TabPage();
            this._taskPanel = new Node.Controls.Jobs.Tasks.PanelTask();
            this.headerPanel.SuspendLayout();
            this._tabs.SuspendLayout();
            this._tabJob.SuspendLayout();
            this._tabTask.SuspendLayout();
            this.SuspendLayout();
            // 
            // logger
            // 
            this.logger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logger.Location = new System.Drawing.Point(3, 3);
            this.logger.Name = "logger";
            this.logger.Size = new System.Drawing.Size(699, 523);
            this.logger.TabIndex = 0;
            this.logger.threadID = 0;
            // 
            // headerPanel
            // 
            this.headerPanel.Controls.Add(this._headerJob);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(713, 33);
            this.headerPanel.TabIndex = 3;
            // 
            // _controlJob
            // 
            this._headerJob.Dock = System.Windows.Forms.DockStyle.Fill;
            this._headerJob.Location = new System.Drawing.Point(0, 0);
            this._headerJob.Name = "_headerJob";
            this._headerJob.Size = new System.Drawing.Size(713, 33);
            this._headerJob.TabIndex = 0;
            // 
            // _tabs
            // 
            this._tabs.Controls.Add(this._tabJob);
            this._tabs.Controls.Add(this._tabTask);
            this._tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabs.Location = new System.Drawing.Point(0, 33);
            this._tabs.Name = "_tabs";
            this._tabs.SelectedIndex = 0;
            this._tabs.Size = new System.Drawing.Size(713, 555);
            this._tabs.TabIndex = 4;
            // 
            // _tabJob
            // 
            this._tabJob.Controls.Add(this.logger);
            this._tabJob.Location = new System.Drawing.Point(4, 22);
            this._tabJob.Name = "_tabJob";
            this._tabJob.Padding = new System.Windows.Forms.Padding(3);
            this._tabJob.Size = new System.Drawing.Size(705, 529);
            this._tabJob.TabIndex = 0;
            this._tabJob.Text = "Job";
            this._tabJob.UseVisualStyleBackColor = true;
            // 
            // _tabTask
            // 
            this._tabTask.Controls.Add(this._taskPanel);
            this._tabTask.Location = new System.Drawing.Point(4, 22);
            this._tabTask.Name = "_tabTask";
            this._tabTask.Padding = new System.Windows.Forms.Padding(3);
            this._tabTask.Size = new System.Drawing.Size(705, 529);
            this._tabTask.TabIndex = 1;
            this._tabTask.Text = "Tasks";
            this._tabTask.UseVisualStyleBackColor = true;
            // 
            // _taskPanel
            // 
            this._taskPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._taskPanel.Location = new System.Drawing.Point(3, 3);
            this._taskPanel.Name = "_taskPanel";
            this._taskPanel.Size = new System.Drawing.Size(699, 523);
            this._taskPanel.TabIndex = 0;
            // 
            // PanelJob
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._tabs);
            this.Controls.Add(this.headerPanel);
            this.Name = "PanelJob";
            this.Size = new System.Drawing.Size(713, 588);
            this.VisibleChanged += new System.EventHandler(this.onVisibleChanged);
            this.headerPanel.ResumeLayout(false);
            this._tabs.ResumeLayout(false);
            this._tabJob.ResumeLayout(false);
            this._tabTask.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public LoggerControl logger;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.TabControl _tabs;
        private System.Windows.Forms.TabPage _tabJob;
        private System.Windows.Forms.TabPage _tabTask;
        private PanelTask _taskPanel;
        private HeaderJob _headerJob;
    }
}
