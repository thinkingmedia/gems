using System.Collections.Generic;
using System.Data;

namespace Gems.Email.Tasks.Report
{
    public class DataReport
    {
        public string Summary { get; set; }
        public string Title { get; set; }

        public List<DataTable> Tables { get; set; }

        public DataReport(IEnumerable<string> pColumnHeadings)
        {
            DataTable table = new DataTable(pColumnHeadings);

            // all reports will need at least one table
            Tables = new List<DataTable> {table};
        }
    }
}