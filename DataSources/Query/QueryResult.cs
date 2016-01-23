using System;

namespace DataSources.Query
{
    public class QueryResult
    {
        /// <summary>
        /// The last record ID
        /// </summary>
        public readonly UInt32 ID;

        /// <summary>
        /// The number of rows effected by the query
        /// </summary>
        public readonly int Rows;

        /// <summary>
        /// If the query was successful
        /// </summary>
        private readonly bool _success;

        /// <summary>
        /// Collection of records
        /// </summary>
        public Records Records;

        /// <summary>
        /// Constructor
        /// </summary>
        public QueryResult(int pRows, UInt32 pID)
        {
            Rows = pRows;
            ID = pID;
            Records = new Records();
            _success = true;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private QueryResult(Records pRecords)
        {
            Records = pRecords;
            _success = true;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private QueryResult(bool pSuccess)
        {
            Records = new Records();
            _success = pSuccess;
        }

        /// <summary>
        /// Converts to bool
        /// </summary>
        public static implicit operator bool(QueryResult pResult)
        {
            return pResult._success;
        }

        /// <summary>
        /// Converts to records
        /// </summary>
        public static implicit operator Records(QueryResult pResult)
        {
            return pResult.Records;
        }

        /// <summary>
        /// Converts bool to result
        /// </summary>
        public static implicit operator QueryResult(bool pSuccess)
        {
            return new QueryResult(pSuccess);
        }

        /// <summary>
        /// Converts records to result
        /// </summary>
        public static implicit operator QueryResult(Records pRecords)
        {
            return new QueryResult(pRecords);
        }
    }
}