namespace Node.Controls.Jobs.Tasks
{
    partial class HeaderTask
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
            this._tasksList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this._btnRefresh = new System.Windows.Forms.Button();
            this._typeFilter = new System.Windows.Forms.ComboBox();
            this._dateStart = new System.Windows.Forms.DateTimePicker();
            this._dateEnd = new System.Windows.Forms.DateTimePicker();
            this._checkFilter = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // _tasksList
            // 
            this._tasksList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._tasksList.FormattingEnabled = true;
            this._tasksList.Location = new System.Drawing.Point(45, 3);
            this._tasksList.Name = "_tasksList";
            this._tasksList.Size = new System.Drawing.Size(173, 21);
            this._tasksList.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tasks";
            // 
            // _btnRefresh
            // 
            this._btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnRefresh.Location = new System.Drawing.Point(752, 3);
            this._btnRefresh.Name = "_btnRefresh";
            this._btnRefresh.Size = new System.Drawing.Size(75, 23);
            this._btnRefresh.TabIndex = 6;
            this._btnRefresh.Text = "Refresh";
            this._btnRefresh.UseVisualStyleBackColor = true;
            this._btnRefresh.Click += new System.EventHandler(this.onTaskChanged);
            // 
            // _typeFilter
            // 
            this._typeFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._typeFilter.FormattingEnabled = true;
            this._typeFilter.Location = new System.Drawing.Point(224, 3);
            this._typeFilter.Name = "_typeFilter";
            this._typeFilter.Size = new System.Drawing.Size(121, 21);
            this._typeFilter.Sorted = true;
            this._typeFilter.TabIndex = 7;
            this._typeFilter.SelectedIndexChanged += new System.EventHandler(this.onFilterChanged);
            // 
            // _dateStart
            // 
            this._dateStart.Enabled = false;
            this._dateStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dateStart.Location = new System.Drawing.Point(351, 3);
            this._dateStart.Name = "_dateStart";
            this._dateStart.Size = new System.Drawing.Size(104, 20);
            this._dateStart.TabIndex = 8;
            this._dateStart.ValueChanged += new System.EventHandler(this.onFilterChanged);
            // 
            // _dateEnd
            // 
            this._dateEnd.Enabled = false;
            this._dateEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dateEnd.Location = new System.Drawing.Point(461, 3);
            this._dateEnd.Name = "_dateEnd";
            this._dateEnd.Size = new System.Drawing.Size(104, 20);
            this._dateEnd.TabIndex = 9;
            this._dateEnd.ValueChanged += new System.EventHandler(this.onFilterChanged);
            // 
            // _checkFilter
            // 
            this._checkFilter.AutoSize = true;
            this._checkFilter.Location = new System.Drawing.Point(571, 5);
            this._checkFilter.Name = "_checkFilter";
            this._checkFilter.Size = new System.Drawing.Size(15, 14);
            this._checkFilter.TabIndex = 10;
            this._checkFilter.UseVisualStyleBackColor = true;
            this._checkFilter.CheckedChanged += new System.EventHandler(this.onFilterChanged);
            // 
            // HeaderTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._checkFilter);
            this.Controls.Add(this._dateEnd);
            this.Controls.Add(this._dateStart);
            this.Controls.Add(this._typeFilter);
            this.Controls.Add(this._btnRefresh);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._tasksList);
            this.Name = "HeaderTask";
            this.Size = new System.Drawing.Size(830, 27);
            this.VisibleChanged += new System.EventHandler(this.onTaskChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox _tasksList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _btnRefresh;
        private System.Windows.Forms.ComboBox _typeFilter;
        private System.Windows.Forms.DateTimePicker _dateStart;
        private System.Windows.Forms.DateTimePicker _dateEnd;
        private System.Windows.Forms.CheckBox _checkFilter;
    }
}
