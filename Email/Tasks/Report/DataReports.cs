using System.Collections.Generic;

namespace Gems.Email.Tasks.Report
{
    public class DataReports
    {
        public List<DataReport> Reports { get; set; }

        public DataReports()
        {
            Reports = new List<DataReport>();
        }
    }
}