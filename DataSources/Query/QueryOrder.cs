using System.Collections.Generic;

namespace DataSources.Query
{
    public static class QueryOrder
    {
        /// <summary>
        /// Orders the result by a fieldname.
        /// </summary>
        public static iQueryBuilder OrderBy(
            this iQueryBuilder pQuery,
            string pFieldName,
            QueryBuilder.eDIR pDir = QueryBuilder.eDIR.ASC,
            bool pExpression = false)
        {
            QueryBuilder query = (QueryBuilder)pQuery;
            string str = pExpression ? pFieldName : query.Field(pFieldName);
            KeyValuePair<QueryBuilder.eDIR, string> pair = new KeyValuePair<QueryBuilder.eDIR, string>(pDir, str);
            query.Order.Add(pair);
            return pQuery;
        }
    }
}