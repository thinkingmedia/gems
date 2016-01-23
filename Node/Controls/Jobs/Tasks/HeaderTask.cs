using System;
using System.Collections.Generic;
using System.Linq;
using Common.Events;
using Jobs.Reports;
using Jobs.Tasks.Events;

namespace Node.Controls.Jobs.Tasks
{
    public partial class HeaderTask : JobControl
    {
        /// <summary>
        /// The source list of events.
        /// </summary>
        private readonly List<iEventObject> _events;

        /// <summary>
        /// A list of task info objects.
        /// </summary>
        private readonly List<iTaskReport> _tasks;

        /// <summary>
        /// Broadcasts the list of event objects that should be shown in the UI.
        /// </summary>
        public Action<List<iEventObject>> EventObjects;

        /// <summary>
        /// Filters the event objects and broadcasts the list.
        /// </summary>
        private void FilterChanged()
        {
            IEnumerable<iEventObject> filter = _events;

            if (_typeFilter.SelectedIndex != 0)
            {
                filter = from e in filter 
                         where String.CompareOrdinal(e.Type, _typeFilter.Text) == 0 
                         select e;
            }

            if (_checkFilter.Checked)
            {
                filter = from e in filter
                         where e.When.Date >= _dateStart.Value .Date
                            && e.When.Date <= _dateEnd.Value.Date
                         select e;
            }

            FireEvents.Action(EventObjects, filter.ToList());
        }

        /// <summary>
        /// Refreshes the data for the control.
        /// </summary>
        private void UpdateControl()
        {
            if (!Configured || !Visible)
            {
                return;
            }

            iJobReport jobReport = JobService.getJobReport(JobID, true);
            Guid taskID = ActiveJobService.getActiveTask();

            _tasks.Clear();
            _tasks.AddRange(jobReport.Tasks);
            iTaskReport task = _tasks.FirstOrDefault(pTask=>pTask.ID == taskID);

            UpdateTasksList(taskID);

            _events.Clear();
            if (task != null)
            {
                _events.AddRange(task.EventRecorder.getEvents());
            }

            UpdateFilterList();
            UpdateDateRange();

            FilterChanged();
        }

        /// <summary>
        /// Filtering of events by a date range.
        /// </summary>
        private void UpdateDateRange()
        {
            _dateStart.Enabled = false;
            _dateEnd.Enabled = false;
            _checkFilter.Enabled = true;
            _checkFilter.Checked = false;

            if (_events.Count == 0)
            {
                return;
            }

            DateTime start = (from e in _events orderby e.When select e.When).First();
            DateTime end = (from e in _events orderby e.When descending select e.When).First();

            _dateEnd.MinDate = start;
            _dateStart.MaxDate = end;

            _dateEnd.MinDate = start;
            _dateStart.MaxDate = end;
        }

        /// <summary>
        /// Filtering events by their type.
        /// </summary>
        private void UpdateFilterList()
        {
            _typeFilter.Items.Clear();
            _typeFilter.Items.Add("-- All Types --");
            _typeFilter.SelectedIndex = 0;

            foreach (string type in from e in _events group e by e.Type into g select g.Key)
            {
                _typeFilter.Items.Add(type);
            }
        }

        /// <summary>
        /// Updates the tasks drop down list.
        /// </summary>
        private void UpdateTasksList(Guid pTaskID)
        {
            _tasksList.Items.Clear();
            _tasksList.Items.Add("-- All Events --");
            int selected = 0;
            foreach (iTaskReport task in _tasks)
            {
                _tasksList.Items.Add(task.Name);
                if (task.ID == pTaskID)
                {
                    selected = _tasksList.Items.Count - 1;
                }
            }
            _tasksList.SelectedIndex = selected;
        }

        /// <summary>
        /// Filter events by their type.
        /// </summary>
        private void onFilterChanged(object pSender, EventArgs pEventArgs)
        {
            _dateStart.Enabled = _checkFilter.Checked;
            _dateEnd.Enabled = _checkFilter.Checked;

            FilterChanged();
        }

        /// <summary>
        /// Updates the dropdown when a task is selected in another control.
        /// </summary>
        private void onTaskChanged(Guid pTaskID)
        {
            UpdateControl();
        }

        /// <summary>
        /// When the control becomes visible it should be updated.
        /// </summary>
        private void onTaskChanged(object pSender, EventArgs pEventArgs)
        {
            UpdateControl();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HeaderTask()
        {
            InitializeComponent();

            _tasks = new List<iTaskReport>();
            _events = new List<iEventObject>();
        }

        /// <summary>
        /// Assigns a Job ID to the control.
        /// </summary>
        protected override void onJobAssigned()
        {
            ActiveJobService.TaskChanged += onTaskChanged;

            UpdateControl();
        }
    }
}