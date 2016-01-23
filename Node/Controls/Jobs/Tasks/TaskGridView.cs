using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Jobs.Tasks.Events;

namespace Node.Controls.Jobs.Tasks
{
    public partial class TaskGridView : JobControl
    {
        private const string _COLUMN_ID = "ID";
        private const string _COLUMN_DESC = "Description";
        private const string _COLUMN_SEVERITY = "Severity";
        private const string _COLUMN_TYPE = "Type";
        private const string _COLUMN_WHEN = "When";

        /// <summary>
        /// Contains the report data.
        /// </summary>
        private readonly DataTable _dataTable;

        /// <summary>
        /// The list of events being viewed.
        /// </summary>
        private readonly Dictionary<Guid,iEventObject> _events;

        /// <summary>
        /// Creates an empty data table for displaying info about jobs.
        /// </summary>
        private static DataTable CreateDataTable()
        {
            DataTable dataTable = new DataTable("Events");

            DataColumn id = new DataColumn { ColumnName = _COLUMN_ID, DataType = typeof(Guid)};
            DataColumn severity = new DataColumn { ColumnName = _COLUMN_SEVERITY, DataType = Type.GetType("System.String") };
            DataColumn when = new DataColumn {ColumnName = _COLUMN_WHEN, DataType = typeof (DateTime)};
            DataColumn type = new DataColumn {ColumnName = _COLUMN_TYPE, DataType = Type.GetType("System.String")};
            DataColumn desc = new DataColumn {ColumnName = _COLUMN_DESC, DataType = Type.GetType("System.String")};

            dataTable.Columns.Add(id);
            dataTable.Columns.Add(severity);
            dataTable.Columns.Add(when);
            dataTable.Columns.Add(type);
            dataTable.Columns.Add(desc);

            return dataTable;
        }

        /// <summary>
        /// Updates the list of events.
        /// </summary>
        private void UpdateEventList()
        {
            _dataTable.Clear();

            foreach (iEventObject evnt in _events.Values.OrderByDescending(pEvent=>pEvent.When))
            {
                DataRow row = _dataTable.NewRow();
                row[_COLUMN_ID] = evnt.ID;
                row[_COLUMN_SEVERITY] = evnt.Severity.ToString();
                row[_COLUMN_WHEN] = evnt.When;
                row[_COLUMN_TYPE] = evnt.Type;
                row[_COLUMN_DESC] = evnt.Desc;

                _dataTable.Rows.Add(row);
            }
        }

        /// <summary>
        /// Gets a column and ensures it's not null.
        /// </summary>
        private DataGridViewColumn get(string pColumnName)
        {
            DataGridViewColumn column = view.Columns[pColumnName];
            if (column == null)
            {
                throw new NullReferenceException(string.Format("Column [{0}] is not defined.", pColumnName));
            }
            return column;
        }

        /// <summary>
        /// Update the event content view.
        /// </summary>
        private void onSelectionChanged(object pSender, EventArgs pEventArgs)
        {
            content.Items.Clear();

            int selectedRow = view.SelectedRows.Count == 1
                ? view.SelectedRows[0].Index
                : ((view.SelectedCells.Count == 1)
                    ? view.SelectedCells[0].RowIndex
                    : - 1);

            if (selectedRow == -1)
            {
                return;
            }

            using (Graphics g = content.CreateGraphics())
            {
                int width = 0;
                Guid id = (Guid)view.Rows[selectedRow].Cells[_COLUMN_ID].Value;
                foreach (string line in _events[id].Message.Split(new[] { '\n' }))
                {
                    content.Items.Add(line);
                    width = Math.Max((int)g.MeasureString(line, content.Font).Width, width);
                }
                content.HorizontalExtent = width + SystemInformation.HorizontalScrollBarHeight +
                                           SystemInformation.VerticalScrollBarWidth;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TaskGridView()
        {
            InitializeComponent();

            view.DataSource = _dataTable = CreateDataTable();
            _events = new Dictionary<Guid, iEventObject>();

            get(_COLUMN_SEVERITY).Width = 75;
            get(_COLUMN_WHEN).Width = 150;
            get(_COLUMN_WHEN).DefaultCellStyle.Format = "G";
            get(_COLUMN_TYPE).Width = 100;
            get(_COLUMN_DESC).Width = 500;
        }

        /// <summary>
        /// Assigns a list of event objects to be viewed.
        /// </summary>
        public void setEventObjects(IEnumerable<iEventObject> pEvents)
        {
            _events.Clear();
            foreach (iEventObject evnt in pEvents)
            {
                _events.Add(evnt.ID, evnt);
            }

            UpdateEventList();
        }
    }
}