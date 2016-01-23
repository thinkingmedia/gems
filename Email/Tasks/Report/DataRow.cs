using System.Collections.Generic;
using System.Linq;

namespace Gems.Email.Tasks.Report
{
    public class DataRow
    {
        public List<DataValue> Cells { get; set; }

        public DataRow(IEnumerable<string> pRowValues)
        {
            Cells = (from str in pRowValues select new DataValue(str)).ToList();
        }
    }
}