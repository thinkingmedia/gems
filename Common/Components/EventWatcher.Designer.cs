namespace Common.Components
{
    partial class EventWatcher
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param _name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /// <param name="disposing"></param>
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
            this.lvEvents = new System.Windows.Forms.ListView();
            this.col_when = new System.Windows.Forms.ColumnHeader();
            this.col_type = new System.Windows.Forms.ColumnHeader();
            this.col_desc = new System.Windows.Forms.ColumnHeader();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.comboLimit = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnTestEMail = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvEvents
            // 
            this.lvEvents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_when,
            this.col_type,
            this.col_desc});
            this.lvEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvEvents.FullRowSelect = true;
            this.lvEvents.GridLines = true;
            this.lvEvents.HideSelection = false;
            this.lvEvents.Location = new System.Drawing.Point(0, 32);
            this.lvEvents.MultiSelect = false;
            this.lvEvents.Name = "lvEvents";
            this.lvEvents.Size = new System.Drawing.Size(788, 614);
            this.lvEvents.TabIndex = 11;
            this.lvEvents.UseCompatibleStateImageBehavior = false;
            this.lvEvents.View = System.Windows.Forms.View.Details;
            // 
            // col_when
            // 
            this.col_when.Text = "When";
            this.col_when.Width = 150;
            // 
            // col_type
            // 
            this.col_type.Text = "Type";
            this.col_type.Width = 80;
            // 
            // col_desc
            // 
            this.col_desc.Text = "Description";
            this.col_desc.Width = 300;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnTestEMail);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.comboLimit);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(788, 32);
            this.panel1.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(600, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "Limit:";
            // 
            // comboLimit
            // 
            this.comboLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboLimit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboLimit.FormattingEnabled = true;
            this.comboLimit.Items.AddRange(new object[] {
            "25",
            "50",
            "100",
            "200",
            "500",
            "ALL"});
            this.comboLimit.Location = new System.Drawing.Point(641, 3);
            this.comboLimit.Name = "comboLimit";
            this.comboLimit.Size = new System.Drawing.Size(63, 21);
            this.comboLimit.TabIndex = 30;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(710, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 29;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(3, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 28;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnTestEMail
            // 
            this.btnTestEMail.Enabled = false;
            this.btnTestEMail.Location = new System.Drawing.Point(84, 3);
            this.btnTestEMail.Name = "btnTestEMail";
            this.btnTestEMail.Size = new System.Drawing.Size(75, 23);
            this.btnTestEMail.TabIndex = 32;
            this.btnTestEMail.Text = "Test EMail";
            this.btnTestEMail.UseVisualStyleBackColor = true;
            this.btnTestEMail.Click += new System.EventHandler(this.btnTestEMail_Click);
            // 
            // EventWatcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvEvents);
            this.Controls.Add(this.panel1);
            this.Name = "EventWatcher";
            this.Size = new System.Drawing.Size(788, 646);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvEvents;
        private System.Windows.Forms.ColumnHeader col_when;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ColumnHeader col_type;
        private System.Windows.Forms.ColumnHeader col_desc;
        private System.Windows.Forms.ComboBox comboLimit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnTestEMail;
    }
}
