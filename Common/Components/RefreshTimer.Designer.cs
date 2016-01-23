namespace Common.Components
{
    partial class RefreshTimer
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param _name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.components = new System.ComponentModel.Container();
            this.check_auto = new System.Windows.Forms.CheckBox();
            this.combo_rate = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClientRefresh = new System.Windows.Forms.Button();
            this.refresh_timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // check_auto
            // 
            this.check_auto.AutoSize = true;
            this.check_auto.Checked = true;
            this.check_auto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.check_auto.Location = new System.Drawing.Point(208, 5);
            this.check_auto.Name = "check_auto";
            this.check_auto.Size = new System.Drawing.Size(88, 17);
            this.check_auto.TabIndex = 33;
            this.check_auto.Text = "Auto Refresh";
            this.check_auto.UseVisualStyleBackColor = true;
            // 
            // combo_rate
            // 
            this.combo_rate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_rate.FormattingEnabled = true;
            this.combo_rate.Items.AddRange(new object[] {
            "1/2 Second",
            "1 Second",
            "5 Seconds",
            "10 Seconds",
            "30 Seconds"});
            this.combo_rate.Location = new System.Drawing.Point(81, 3);
            this.combo_rate.Name = "combo_rate";
            this.combo_rate.Size = new System.Drawing.Size(121, 21);
            this.combo_rate.TabIndex = 32;
            this.combo_rate.SelectedIndexChanged += new System.EventHandler(this.combo_rate_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "Refresh Rate:";
            // 
            // btnClientRefresh
            // 
            this.btnClientRefresh.Location = new System.Drawing.Point(302, 1);
            this.btnClientRefresh.Name = "btnClientRefresh";
            this.btnClientRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnClientRefresh.TabIndex = 30;
            this.btnClientRefresh.Text = "Refresh";
            this.btnClientRefresh.UseVisualStyleBackColor = true;
            this.btnClientRefresh.Click += new System.EventHandler(this.btnClientRefresh_Click);
            // 
            // refresh_timer
            // 
            this.refresh_timer.Tick += new System.EventHandler(this.refresh_timer_Tick);
            // 
            // RefreshTimer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.check_auto);
            this.Controls.Add(this.combo_rate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClientRefresh);
            this.Name = "RefreshTimer";
            this.Size = new System.Drawing.Size(380, 28);
            this.VisibleChanged += new System.EventHandler(this.RefreshTimer_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClientRefresh;
        private System.Windows.Forms.Timer refresh_timer;
        public System.Windows.Forms.CheckBox check_auto;
        private System.Windows.Forms.ComboBox combo_rate;
    }
}
