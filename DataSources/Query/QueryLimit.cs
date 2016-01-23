namespace DataSources.Query
{
    public static class QueryLimit
    {
        public static iQueryBuilder Limit(this iQueryBuilder pQuery, int pLimit)
        {
            QueryBuilder query = (QueryBuilder)pQuery;
            query.Limit = pLimit;
            return pQuery;
        }

        public static iQueryBuilder Limit(this iQueryBuilder pQuery, int pLimit, int pOffset)
        {
            QueryBuilder query = (QueryBuilder)pQuery;
            query.Limit = pLimit;
            query.Limit = pOffset;
            return pQuery;
        }

        public static iQueryBuilder Offset(this iQueryBuilder pQuery, int pOffset)
        {
            QueryBuilder query = (QueryBuilder)pQuery;
            query.Offset = pOffset;
            return pQuery;
        }
    }
}