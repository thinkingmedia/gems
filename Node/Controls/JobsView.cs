using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Common.Events;
using Jobs;
using Jobs.Reports;
using Jobs.Tasks.Events;
using Timer = System.Timers.Timer;

namespace Node.Controls
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed partial class JobsView : UserControl, iActiveJobService, iJobsView
    {
        private const string _COLUMN_ERRORS = "Errors";
        private const string _COLUMN_ID = "ID";
        private const string _COLUMN_PLUGIN = "Plug-in";
        private const string _COLUMN_NAME = "Name";
        private const string _COLUMN_STATUS = "Status";
        private const string _COLUMN_TIMER = "Timer";

        /// <summary>
        /// Number of columns that display job details.
        /// </summary>
        private const int _INFO_COLUMNS = 6;

        /// <summary>
        /// A bold font.
        /// </summary>
        private readonly Font _boldFont;

        /// <summary>
        /// Contains the report data.
        /// </summary>
        private readonly DataTable _dataTable;

        /// <summary>
        /// The engine service.
        /// </summary>
        private readonly iEngineService _engineService;

        /// <summary>
        /// The job service.
        /// </summary>
        private readonly iJobService _jobService;

        /// <summary>
        /// The popup menu to show for jobs.
        /// </summary>
        private ContextMenuStrip _jobMenu;

        /// <summary>
        /// True to handle selection change events.
        /// </summary>
        private bool _refreshing;

        /// <summary>
        /// The popup menu to show for tasks.
        /// </summary>
        private ContextMenuStrip _taskMenu;

        /// <summary>
        /// Creates an empty data table for displaying info about jobs.
        /// </summary>
        private static DataTable CreateDataTable()
        {
            DataTable dataTable = new DataTable("Report");

            DataColumn id = new DataColumn {ColumnName = _COLUMN_ID, DataType = typeof (Guid), Unique = true};
            DataColumn plugin = new DataColumn { ColumnName = _COLUMN_PLUGIN, DataType = Type.GetType("System.String") };
            DataColumn name = new DataColumn { ColumnName = _COLUMN_NAME, DataType = Type.GetType("System.String") };
            DataColumn status = new DataColumn { ColumnName = _COLUMN_STATUS, DataType = typeof(iJobReport) };
            DataColumn errors = new DataColumn {ColumnName = _COLUMN_ERRORS, DataType = typeof (iJobReport)};
            DataColumn timer = new DataColumn {ColumnName = _COLUMN_TIMER, DataType = typeof (iJobReport)};

            dataTable.Columns.Add(id);
            dataTable.Columns.Add(plugin);
            dataTable.Columns.Add(name);
            dataTable.Columns.Add(status);
            dataTable.Columns.Add(errors);
            dataTable.Columns.Add(timer);

            // set which column is used to find rows
            dataTable.PrimaryKey = new[] {id};

            return dataTable;
        }

        /// <summary>
        /// Adds extra columns as their needed.
        /// </summary>
        private void EnsureColumnsExist(int pTaskCount)
        {
            int max = pTaskCount - (_dataTable.Columns.Count - _INFO_COLUMNS);
            for (int i = 0; i < max; i++)
            {
                DataColumn column = new DataColumn
                                    {
                                        ColumnName = string.Format("Task:{0}", _dataTable.Columns.Count - _INFO_COLUMNS),
                                        DataType = typeof (iTaskReport)
                                    };
                _dataTable.Columns.Add(column);
            }
        }

        /// <summary>
        /// Updates the report view.
        /// </summary>
        private void RefreshTable()
        {
            if (!_engineService.isRunning() || _refreshing)
            {
                return;
            }

            _refreshing = true;

            // Check if there are any jobs to report on.
            List<iJobReport> reports = (from guid in _jobService.getJobIDs()
                                        select _jobService.getJobReport(guid, true)).ToList();

            foreach (iJobReport info in reports)
            {
                EnsureColumnsExist(info.Tasks.Count);

                if (!_dataTable.Rows.Contains(info.ID))
                {
                    DataRow newRow = _dataTable.NewRow();
                    newRow[0] = info.ID;
                    newRow[1] = info.Plugin;
                    newRow[2] = info.Name;
                    _dataTable.Rows.Add(newRow);
                }

                DataRow row = _dataTable.Rows.Find(info.ID);

                row[_COLUMN_STATUS] = info;
                row[_COLUMN_ERRORS] = info;
                row[_COLUMN_TIMER] = info;

                for (int i = 0, c = info.Tasks.Count; i < c; i++)
                {
                    row[i + _INFO_COLUMNS] = info.Tasks[i];
                }
            }

            _refreshing = false;
        }

        /// <summary>
        /// Formats the display of a cell.
        /// </summary>
        /// <param name="pSender"></param>
        /// <param name="pArgs"></param>
        private void onCellFormat(object pSender, DataGridViewCellFormattingEventArgs pArgs)
        {
            pArgs.FormattingApplied = true;

            iJobReport jobReport = pArgs.Value as iJobReport;
            iTaskReport taskReportReadonly = pArgs.Value as iTaskReport;

            DataColumn column = _dataTable.Columns[pArgs.ColumnIndex];

            DataGridViewCellStyle style = pArgs.CellStyle;

            switch (column.ColumnName)
            {
                case _COLUMN_STATUS:
                    if (jobReport != null)
                    {
                        style.ForeColor = JobInfoTheme.ForeColor(JobInfoTheme.eTHEME_ELEMENT.STATE, jobReport);
                        style.BackColor = JobInfoTheme.BackColor(JobInfoTheme.eTHEME_ELEMENT.STATE, jobReport);
                        style.Font = _boldFont;
                        style.Alignment = DataGridViewContentAlignment.MiddleRight;
                        pArgs.Value = JobInfoTheme.StateText(jobReport);
                    }
                    break;

                case _COLUMN_ERRORS:
                    if (jobReport != null)
                    {
                        style.ForeColor = JobInfoTheme.ForeColor(JobInfoTheme.eTHEME_ELEMENT.ERRORS, jobReport);
                        style.BackColor = JobInfoTheme.BackColor(JobInfoTheme.eTHEME_ELEMENT.ERRORS, jobReport);
                        if (jobReport.Errors > 0)
                        {
                            style.Font = _boldFont;
                        }
                        style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        pArgs.Value = JobInfoTheme.ErrorsText(jobReport);
                    }
                    break;

                case _COLUMN_TIMER:
                    if (jobReport != null)
                    {
                        style.ForeColor = JobInfoTheme.ForeColor(JobInfoTheme.eTHEME_ELEMENT.TIMER, jobReport);
                        style.BackColor = JobInfoTheme.BackColor(JobInfoTheme.eTHEME_ELEMENT.TIMER, jobReport);
                        style.Alignment = DataGridViewContentAlignment.MiddleRight;
                        style.Font = _boldFont;
                        pArgs.Value = JobInfoTheme.TimerText(jobReport);
                    }
                    break;

                default:
                    if (taskReportReadonly != null)
                    {
                        iEventRecorder eventRecorder = taskReportReadonly.EventRecorder;

                        if (eventRecorder.getCount() == 0)
                        {
                            style.BackColor = Color.Green;
                            style.ForeColor = Color.White;
                            pArgs.Value = taskReportReadonly.Name;
                        }
                        else if (eventRecorder.getCount(eEVENT_SEVERITY.UNHANDLED) > 0)
                        {
                            style.BackColor = Color.Red;
                            style.ForeColor = Color.White;
                            pArgs.Value = string.Format("{0} - ({1}/{2})", taskReportReadonly.Name,
                                eventRecorder.getCount(eEVENT_SEVERITY.UNHANDLED),
                                eventRecorder.getCount(eEVENT_SEVERITY.HANDLED));
                        }
                        else
                        {
                            style.BackColor = Color.Yellow;
                            style.ForeColor = Color.Black;
                            pArgs.Value = string.Format("{0} ({1})", taskReportReadonly.Name,
                                eventRecorder.getCount(eEVENT_SEVERITY.HANDLED));
                        }
                    }
                    break;
            }

            pArgs.Value = pArgs.Value.ToString();
        }

        /// <summary>
        /// When a cell is selected.
        /// </summary>
        private void onCellSelected(object pSender, EventArgs pEventArgs)
        {
            if (_refreshing)
            {
                return;
            }

            DataGridViewCellCollection cells = null;
            iTaskReport task = null;

            if (view.SelectedRows.Count == 1)
            {
                cells = view.SelectedRows[0].Cells;
            }
            else if (view.SelectedCells.Count == 1)
            {
                int row = view.SelectedCells[0].RowIndex;
                cells = view.Rows[row].Cells;
                task = view.SelectedCells[0].Value as iTaskReport;
            }

            if (cells == null || !_engineService.isRunning())
            {
                return;
            }

            FireEvents.Action(JobChanged, (Guid)cells[0].Value);
            FireEvents.Action(TaskChanged, task != null ? task.ID : Guid.Empty);
        }

        /// <summary>
        /// Gets the context menu for a job or task.
        /// </summary>
        private void onContextMenuNeeded(object pSender,
                                         DataGridViewCellContextMenuStripNeededEventArgs pEvent)
        {
            view.Rows[pEvent.RowIndex].Cells[pEvent.ColumnIndex].Selected = true;
            pEvent.ContextMenuStrip = pEvent.ColumnIndex < _INFO_COLUMNS
                ? _jobMenu
                : _taskMenu;
        }

        /// <summary>
        /// Start the timer once the engine has started.
        /// </summary>
        private void onEngineStart(object pSender, EventArgs pEventArgs)
        {
            Timer timer = new Timer {Interval = 1000, Enabled = true};
            timer.Elapsed += (pSender2, pArgs2)=>FireEvents.Invoke(this, RefreshTable);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public JobsView(iEngineService pEngineService, iJobService pJobService)
        {
            _engineService = pEngineService;
            _jobService = pJobService;

            InitializeComponent();

            view.DataSource = _dataTable = CreateDataTable();

            _boldFont = new Font(Font, FontStyle.Bold);

            // create a row for the Main tab
            DataRow main = _dataTable.NewRow();
            main[0] = Guid.Empty;
            main[1] = "Core";
            main[2] = "Main";
            _dataTable.Rows.Add(main);

            DataGridViewColumn column = view.Columns[_COLUMN_ID];
            if (column != null)
            {
                column.Visible = false;
            }

            _engineService.onStart += onEngineStart;
        }

        [Category("Action")]
        [Description("When the current selected job changes.")]
        public event Action<Guid> JobChanged;

        [Category("Action")]
        [Description("When a task for a job is selected.")]
        public event Action<Guid> TaskChanged;

        /// <summary>
        /// The currently selected Job in the UI.
        /// </summary>
        /// <returns>The ID or Guid.Empty.</returns>
        public Guid getActiveJob()
        {
            if (view.SelectedCells.Count != 1)
            {
                return Guid.Empty;
            }
            return (Guid)view.SelectedCells[0].OwningRow.Cells[_COLUMN_ID].Value;
        }

        /// <summary>
        /// The currently selected task for the selected Job.
        /// </summary>
        /// <returns>The ID or Guid.Empty.</returns>
        public Guid getActiveTask()
        {
            if (view.SelectedCells.Count != 1)
            {
                return Guid.Empty;
            }

            iTaskReport reportReadonly = view.SelectedCells[0].Value as iTaskReport;
            if (reportReadonly == null)
            {
                return Guid.Empty;
            }
            return reportReadonly.ID;
        }

        /// <summary>
        /// Selects a job in the report to display details about that job.
        /// </summary>
        public void setActiveJob(Guid pJobID)
        {
            RefreshTable();
            DataRow row = _dataTable.Rows.Find(pJobID);
            if (row == null)
            {
                return;
            }
            view.Rows[_dataTable.Rows.IndexOf(row)].Cells[1].Selected = true;
        }

        /// <summary>
        /// The UI control to be shown.
        /// </summary>
        public Control getControl()
        {
            return this;
        }

        /// <summary>
        /// Assign the menu to use for a selected job.
        /// </summary>
        public void setJobMenu(ContextMenuStrip pMenu)
        {
            _jobMenu = pMenu;
        }

        /// <summary>
        /// Assign the menu to use for a selected task.
        /// </summary>
        public void setTaskMenu(ContextMenuStrip pMenu)
        {
            _taskMenu = pMenu;
        }
    }
}