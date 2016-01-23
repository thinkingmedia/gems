namespace Node.Controls.Jobs.Tasks
{
    partial class PanelTask
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
            this._taskGridView = new Node.Controls.Jobs.Tasks.TaskGridView();
            this._taskDetail = new Node.Controls.Jobs.Tasks.HeaderTask();
            this.SuspendLayout();
            // 
            // _taskGridView
            // 
            this._taskGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._taskGridView.Location = new System.Drawing.Point(0, 29);
            this._taskGridView.Name = "_taskGridView";
            this._taskGridView.Size = new System.Drawing.Size(783, 583);
            this._taskGridView.TabIndex = 1;
            // 
            // _taskDetail
            // 
            this._taskDetail.Dock = System.Windows.Forms.DockStyle.Top;
            this._taskDetail.Location = new System.Drawing.Point(0, 0);
            this._taskDetail.Name = "_taskDetail";
            this._taskDetail.Size = new System.Drawing.Size(783, 29);
            this._taskDetail.TabIndex = 0;
            // 
            // PanelTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._taskGridView);
            this.Controls.Add(this._taskDetail);
            this.Name = "PanelTask";
            this.Size = new System.Drawing.Size(783, 612);
            this.VisibleChanged += new System.EventHandler(this.onVisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private HeaderTask _taskDetail;
        private TaskGridView _taskGridView;

    }
}
