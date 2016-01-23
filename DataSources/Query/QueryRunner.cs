using System;
using System.Collections.Generic;
using System.Linq;
using DataSources.Exceptions;

namespace DataSources.Query
{
    public static class QueryRunner
    {
        /// <summary>
        /// Runs the query with the data source.
        /// </summary>
        /// <param name="pQuery"></param>
        /// <returns></returns>
        private static QueryResult Run(iQueryBuilder pQuery)
        {
            QueryBuilder query = (QueryBuilder)pQuery;
            return query.Model().DataSource.Execute(query);
        }

        /// <summary>
        /// Executes a non-result based query (UPDATE,DELETE)
        /// </summary>
        /// <return>The number of rows changed.</return>
        public static int Execute(this iQueryBuilder pQuery)
        {
            return Run(pQuery).Rows;
        }

        /// <summary>
        /// Executes a query and returns the LAST ID used.
        /// </summary>
        public static UInt32 ExecuteLastID(this iQueryBuilder pQuery)
        {
            return Run(pQuery).ID;
        }

        /// <summary>
        /// Executes the query and returns all the records.
        /// </summary>
        public static Records All(this iQueryBuilder pQuery)
        {
            return Run(pQuery).Records;
        }

        /// <summary>
        /// Executes the query as a list of the IDs/Display fields.
        /// </summary>
        public static Dictionary<UInt32, string> List(this iQueryBuilder pQuery)
        {
            QueryBuilder query = (QueryBuilder)pQuery;
            query.Fields().Clear();
            query.Fields().Add(query.PrimaryKey());
            query.Fields().Add(query.DisplayField());
            return All(pQuery).ToDictionary(pKey => pKey.ID, pValue => pValue.getString(query.DisplayField()));
        }

        /// <summary>
        /// Executes the query for the first record.
        /// </summary>
        public static Record One(this iQueryBuilder pQuery)
        {
            QueryBuilder query = (QueryBuilder)pQuery;
            query.Limit = 1;
            Records records = All(query);
            return records.Count == 1 ? records.First() : null;
        }

        /// <summary>
        /// Returns the value of the first row's first column.
        /// </summary>
        public static T Scalar<T>(this iQueryBuilder pQuery)
        {
            List<T> records = Scalars<T>(pQuery);
            if (records.Count > 1)
            {
                throw new ModelException("Scalar method does not work on queries that return multiple rows.");
            }
            return records.Count == 0 ? default(T) : records[0];
        }

        /// <summary>
        /// Returns the value of the first column as a collection.
        /// </summary>
        public static List<T> Scalars<T>(this iQueryBuilder pQuery)
        {
            Records records = Run(pQuery) ?? new Records();
            return (
                from record in records
                let field = record.Count > 0 ? record[0] : null
                let value = Convert.GetTypeCode(field) == TypeCode.DBNull ||
                            Convert.GetTypeCode(field) == TypeCode.Empty
                    ? default(T)
                    : field
                select (T)Convert.ChangeType(value,typeof(T)))
                .ToList();
        }
    }
}