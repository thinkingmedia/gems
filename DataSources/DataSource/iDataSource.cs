using System;
using DataSources.Query;

namespace DataSources.DataSource
{
    /// <summary>
    /// Defines a _dataSource_ref.
    /// </summary>
    public interface iDataSource : IDisposable
    {
        /// <summary>
        /// Open the data source (may be called more then once).
        /// </summary>
        void Open();

        /// <summary>
        /// Close the data source.
        /// </summary>
        void Close();

        /// <summary>
        /// Returns the name of the current database.
        /// </summary>
        string Name();

        /// <summary>
        /// Generates a valid field name.
        /// </summary>
        string Field(string pTable, string pField);

        /// <summary>
        /// Executes a SQL statement and returns the result set.
        /// </summary>
        [Obsolete]
        Records Execute(string pSql);

        /// <summary>
        /// Executes SQL statement that doesn't require a record set (UPDATE, DELETE).
        /// </summary>
        [Obsolete]
        int ExecuteNonQuery(string pSql);

        /// <summary>
        /// Returns TRUE/FALSE if the given table exists.
        /// </summary>
        bool Exist(string pTable);

        /// <summary>
        /// Returns TRUE/FALSE if the given field exists in the table.
        /// </summary>
        bool FieldExists(string pTable, string pField);

        /// <summary>
        /// Executes the query and returns the results.
        /// </summary>
        /// <param name="pQuery">The query to execute.</param>
        /// <returns>The query results</returns>
        QueryResult Execute(QueryBuilder pQuery);

        /// <summary>
        /// Start a transaction.
        /// </summary>
        void Begin();

        /// <summary>
        /// Commit the transaction.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        void Rollback();
    }
}
