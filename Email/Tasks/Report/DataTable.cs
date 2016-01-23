using System.Collections.Generic;
using System.Linq;

namespace Gems.Email.Tasks.Report
{
    public class DataTable
    {
        public List<DataValue> Columns { get; set; }

        public List<DataRow> Rows { get; set; }

        public DataTable(IEnumerable<string> pColumnHeadings)
        {
            Columns = (from col in pColumnHeadings select new DataValue(col)).ToList();
            Rows = new List<DataRow>();
        }

        public void Add(IEnumerable<string> pRowValues)
        {
            Rows.Add(new DataRow(pRowValues));
        }
    }
}