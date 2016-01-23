
namespace DataSources.DataSource
{
    /// <summary>
    /// The result of executing a query on a IDataSource.
    /// </summary>
    public class DataSourceResult
    {
        /// <summary>
        /// The last updated ID (read-only).
        /// </summary>
        public uint ID { get; private set; }

        /// <summary>
        /// The number of effected rows (read-only).
        /// </summary>
        public int Rows { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public DataSourceResult(int pRows, uint pID)
        {
            Rows = pRows;
            ID = pID;
        }
    }
}
