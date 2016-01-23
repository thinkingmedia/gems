using System.Collections.Generic;
using System.Linq;
using DataSourceEntity.Models;
using DataSources.DataSource;

namespace DataSourceEntity.Schemas
{
    /// <summary>
    /// Handles the reading of a database schema
    /// </summary>
    public class SchemaReader
    {
        /// <summary>
        /// The database connection
        /// </summary>
        private readonly iDataSource _dataSource;

        /// <summary>
        /// Constructor
        /// </summary>
        public SchemaReader(string pAddress, string pUsername, string pPassword)
        {
            DataSources.DataSource.DataSources.Instance.Creators.Register("SCHEMA", ()=>new MySqlDataSource(
                pAddress,
                "INFORMATION_SCHEMA",
                pUsername,
                pPassword
                ));
            DataSources.DataSource.DataSources.Instance.setDefault("SCHEMA");
            _dataSource = DataSources.DataSource.DataSources.Instance.Create("SCHEMA");
        }

        /// <summary>
        /// Reads the schema of the database from the MySQL server.
        /// </summary>
        /// <param name="pDatabase">Name of the database to read.</param>
        /// <param name="pSkip">List of prefix tables to ignore.</param>
        public List<SchemaTable> Read(string pDatabase, string[] pSkip)
        {
            try
            {
                _dataSource.Open();

                Table table = new Table(_dataSource);
                Column column = new Column(_dataSource);

                List<SchemaTable> schemas = new List<SchemaTable>();
                IEnumerable<string> tableNames = table.getTables(pDatabase);
                foreach (string tableName in tableNames)
                {
                    if (pSkip.Any(pTableName=>tableName.StartsWith(pTableName)))
                    {
                        continue;
                    }
                    SchemaTable schema = column.getSchema(pDatabase, tableName);
                    schemas.Add(schema);
                }

                return schemas;
            }
            finally
            {
                _dataSource.Close();
            }
        }
    }
}