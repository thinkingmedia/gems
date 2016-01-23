using System.Collections.Generic;
using System.Linq;
using DataSources;
using DataSources.DataSource;
using DataSources.Query;

namespace DataSourceEntity.Models
{
    /// <summary>
    /// This model is used to access the MySQL internal TABLES
    /// table to find a list of tables for a database.
    /// </summary>
    public class Table : Model
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Table(iDataSource pSource)
            : base(new ModelOptions {Table = "TABLES"}, pSource)
        {
        }

        /// <summary>
        /// Returns a list of the tables in a database.
        /// </summary>
        public IEnumerable<string> getTables(string pDatabase)
        {
            Records records = Select()
                .Where()
                .Eq("TABLE_SCHEMA", pDatabase)
                .End()
                .All();

            return (from record in records select record.getString("TABLE_NAME")).ToList();
        }
    }
}