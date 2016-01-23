using Node.Controls;

namespace Node
{
    sealed partial class Main
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuShutdown = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.splitter = new System.Windows.Forms.SplitContainer();
            this.main_menu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuShutdown2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExit2 = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStayOnTop = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMinimizeOnClose = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHideMinimized = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.jobsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuClearErrors = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSuspendAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuResumeAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStopAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuJob = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuJobClearErrors = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuJobSuspend = new System.Windows.Forms.ToolStripMenuItem();
            this.menuJobResume = new System.Windows.Forms.ToolStripMenuItem();
            this.menuJobStop = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTask = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuTaskClearErrors = new System.Windows.Forms.ToolStripMenuItem();
            this.viewErrorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitter)).BeginInit();
            this.splitter.SuspendLayout();
            this.main_menu.SuspendLayout();
            this.menuJob.SuspendLayout();
            this.menuTask.SuspendLayout();
            this.SuspendLayout();
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.menu;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "E3M-Node";
            this.trayIcon.Visible = true;
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuShutdown,
            this.menuExit});
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(129, 48);
            // 
            // menuShutdown
            // 
            this.menuShutdown.Name = "menuShutdown";
            this.menuShutdown.Size = new System.Drawing.Size(128, 22);
            this.menuShutdown.Tag = "Main.Shutdown";
            this.menuShutdown.Text = "&Shutdown";
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(128, 22);
            this.menuExit.Tag = "Main.Exit";
            this.menuExit.Text = "&Exit";
            // 
            // splitter
            // 
            this.splitter.BackColor = System.Drawing.SystemColors.Control;
            this.splitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitter.Location = new System.Drawing.Point(0, 24);
            this.splitter.Name = "splitter";
            this.splitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitter.Size = new System.Drawing.Size(626, 497);
            this.splitter.SplitterDistance = 144;
            this.splitter.SplitterWidth = 6;
            this.splitter.TabIndex = 2;
            // 
            // main_menu
            // 
            this.main_menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.viewToolStripMenuItem,
            this.jobsToolStripMenuItem});
            this.main_menu.Location = new System.Drawing.Point(0, 0);
            this.main_menu.Name = "main_menu";
            this.main_menu.Size = new System.Drawing.Size(626, 24);
            this.main_menu.TabIndex = 3;
            this.main_menu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator2,
            this.menuShutdown2,
            this.menuExit2});
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem1.Text = "&File";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(125, 6);
            // 
            // menuShutdown2
            // 
            this.menuShutdown2.Name = "menuShutdown2";
            this.menuShutdown2.Size = new System.Drawing.Size(128, 22);
            this.menuShutdown2.Tag = "Main.Shutdown";
            this.menuShutdown2.Text = "&Shutdown";
            // 
            // menuExit2
            // 
            this.menuExit2.Name = "menuExit2";
            this.menuExit2.Size = new System.Drawing.Size(128, 22);
            this.menuExit2.Tag = "Main.Exit";
            this.menuExit2.Text = "E&xit";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStayOnTop,
            this.menuMinimizeOnClose,
            this.menuHideMinimized,
            this.toolStripSeparator1,
            this.menuOptions});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // menuStayOnTop
            // 
            this.menuStayOnTop.Checked = true;
            this.menuStayOnTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuStayOnTop.Name = "menuStayOnTop";
            this.menuStayOnTop.Size = new System.Drawing.Size(190, 22);
            this.menuStayOnTop.Text = "&Stay on top";
            // 
            // menuMinimizeOnClose
            // 
            this.menuMinimizeOnClose.Checked = true;
            this.menuMinimizeOnClose.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuMinimizeOnClose.Name = "menuMinimizeOnClose";
            this.menuMinimizeOnClose.Size = new System.Drawing.Size(190, 22);
            this.menuMinimizeOnClose.Text = "&Minimize on close";
            // 
            // menuHideMinimized
            // 
            this.menuHideMinimized.Checked = true;
            this.menuHideMinimized.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuHideMinimized.Name = "menuHideMinimized";
            this.menuHideMinimized.Size = new System.Drawing.Size(190, 22);
            this.menuHideMinimized.Text = "&Hide when minimized";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(187, 6);
            // 
            // menuOptions
            // 
            this.menuOptions.Name = "menuOptions";
            this.menuOptions.Size = new System.Drawing.Size(190, 22);
            this.menuOptions.Tag = "Main.Options";
            this.menuOptions.Text = "&Options";
            // 
            // jobsToolStripMenuItem
            // 
            this.jobsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuClearErrors,
            this.toolStripSeparator4,
            this.menuSuspendAll,
            this.menuResumeAll,
            this.menuStopAll});
            this.jobsToolStripMenuItem.Name = "jobsToolStripMenuItem";
            this.jobsToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.jobsToolStripMenuItem.Text = "&Jobs";
            // 
            // menuClearErrors
            // 
            this.menuClearErrors.Name = "menuClearErrors";
            this.menuClearErrors.Size = new System.Drawing.Size(152, 22);
            this.menuClearErrors.Tag = "Jobs.ClearErrors.All";
            this.menuClearErrors.Text = "&Clear Errors";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(149, 6);
            // 
            // menuSuspendAll
            // 
            this.menuSuspendAll.Name = "menuSuspendAll";
            this.menuSuspendAll.Size = new System.Drawing.Size(152, 22);
            this.menuSuspendAll.Tag = "Jobs.Suspend.All";
            this.menuSuspendAll.Text = "&Suspend All";
            // 
            // menuResumeAll
            // 
            this.menuResumeAll.Name = "menuResumeAll";
            this.menuResumeAll.Size = new System.Drawing.Size(152, 22);
            this.menuResumeAll.Tag = "Jobs.Resume.All";
            this.menuResumeAll.Text = "&Resume All";
            // 
            // menuStopAll
            // 
            this.menuStopAll.Name = "menuStopAll";
            this.menuStopAll.Size = new System.Drawing.Size(152, 22);
            this.menuStopAll.Tag = "Jobs.Stop.All";
            this.menuStopAll.Text = "Sto&p All";
            // 
            // menuJob
            // 
            this.menuJob.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuJobClearErrors,
            this.toolStripSeparator3,
            this.menuJobSuspend,
            this.menuJobResume,
            this.menuJobStop});
            this.menuJob.Name = "menuJob";
            this.menuJob.Size = new System.Drawing.Size(135, 98);
            // 
            // menuJobClearErrors
            // 
            this.menuJobClearErrors.Name = "menuJobClearErrors";
            this.menuJobClearErrors.Size = new System.Drawing.Size(134, 22);
            this.menuJobClearErrors.Tag = "Jobs.ClearErrors";
            this.menuJobClearErrors.Text = "Clear Errors";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(131, 6);
            // 
            // menuJobSuspend
            // 
            this.menuJobSuspend.Name = "menuJobSuspend";
            this.menuJobSuspend.Size = new System.Drawing.Size(134, 22);
            this.menuJobSuspend.Tag = "Jobs.Suspend";
            this.menuJobSuspend.Text = "Suspend";
            // 
            // menuJobResume
            // 
            this.menuJobResume.Name = "menuJobResume";
            this.menuJobResume.Size = new System.Drawing.Size(134, 22);
            this.menuJobResume.Tag = "Jobs.Resume";
            this.menuJobResume.Text = "Resume";
            // 
            // menuJobStop
            // 
            this.menuJobStop.Name = "menuJobStop";
            this.menuJobStop.Size = new System.Drawing.Size(134, 22);
            this.menuJobStop.Tag = "Jobs.Stop";
            this.menuJobStop.Text = "Stop";
            // 
            // menuTask
            // 
            this.menuTask.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuTaskClearErrors,
            this.viewErrorsToolStripMenuItem});
            this.menuTask.Name = "menuTask";
            this.menuTask.Size = new System.Drawing.Size(135, 48);
            // 
            // menuTaskClearErrors
            // 
            this.menuTaskClearErrors.Name = "menuTaskClearErrors";
            this.menuTaskClearErrors.Size = new System.Drawing.Size(134, 22);
            this.menuTaskClearErrors.Tag = "Jobs.ClearErrors.Task";
            this.menuTaskClearErrors.Text = "&Clear Errors";
            // 
            // viewErrorsToolStripMenuItem
            // 
            this.viewErrorsToolStripMenuItem.Name = "viewErrorsToolStripMenuItem";
            this.viewErrorsToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.viewErrorsToolStripMenuItem.Text = "&View Errors";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(626, 521);
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.main_menu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.main_menu;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Node";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BeforeClose);
            this.Load += new System.EventHandler(this.onWindowLoad);
            this.VisibleChanged += new System.EventHandler(this.onVisibleChanged);
            this.Resize += new System.EventHandler(this.onResized);
            this.menu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitter)).EndInit();
            this.splitter.ResumeLayout(false);
            this.main_menu.ResumeLayout(false);
            this.main_menu.PerformLayout();
            this.menuJob.ResumeLayout(false);
            this.menuTask.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.ToolStripMenuItem menuShutdown;
        private System.Windows.Forms.SplitContainer splitter;
        private System.Windows.Forms.MenuStrip main_menu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuShutdown2;
        private System.Windows.Forms.ToolStripMenuItem menuExit2;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuStayOnTop;
        private System.Windows.Forms.ToolStripMenuItem menuHideMinimized;
        public System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuOptions;
        private System.Windows.Forms.ToolStripMenuItem menuMinimizeOnClose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip menuJob;
        private System.Windows.Forms.ContextMenuStrip menuTask;
        private System.Windows.Forms.ToolStripMenuItem menuJobClearErrors;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuJobSuspend;
        private System.Windows.Forms.ToolStripMenuItem menuJobResume;
        private System.Windows.Forms.ToolStripMenuItem menuJobStop;
        private System.Windows.Forms.ToolStripMenuItem jobsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuClearErrors;
        private System.Windows.Forms.ToolStripMenuItem menuTaskClearErrors;
        private System.Windows.Forms.ToolStripMenuItem viewErrorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuSuspendAll;
        private System.Windows.Forms.ToolStripMenuItem menuResumeAll;
        private System.Windows.Forms.ToolStripMenuItem menuStopAll;
    }
}