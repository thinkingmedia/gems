using System;
using DataSources;
using DataSources.DataSource;
using DataSources.Query;

namespace DataSourcesTest.Mock.DataSource
{
    public class MockDataSource : iDataSource
    {
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Open the data source (may be called more then once).
        /// </summary>
        public void Open()
        {
        }

        /// <summary>
        /// Close the data source.
        /// </summary>
        public void Close()
        {
        }

        /// <summary>
        /// Returns the name of the current database.
        /// </summary>
        public string Name()
        {
            return "Mock";
        }

        /// <summary>
        /// Generates a valid field name.
        /// </summary>
        public string Field(string pTable, string pField)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executes a SQL statement and returns the result set.
        /// </summary>
        public Records Execute(string pSql)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executes SQL statement that doesn't require a record set (UPDATE, DELETE).
        /// </summary>
        public int ExecuteNonQuery(string pSql)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns TRUE/FALSE if the given table exists.
        /// </summary>
        public bool Exist(string pTable)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns TRUE/FALSE if the given field exists in the table.
        /// </summary>
        public bool FieldExists(string pTable, string pField)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executes the query and returns the results.
        /// </summary>
        /// <param name="pQuery">The query to execute.</param>
        /// <returns>The query results</returns>
        public QueryResult Execute(QueryBuilder pQuery)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Start a transaction.
        /// </summary>
        public void Begin()
        {
        }

        /// <summary>
        /// Commit the transaction.
        /// </summary>
        public void Commit()
        {
        }

        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        public void Rollback()
        {
        }
    }
}