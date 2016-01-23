using System;
using System.Collections.Generic;

namespace DataSources.Query
{
    public static class QuerySet
    {
        public static iQueryBuilder Set(this iQueryBuilder pQuery, Record pRecord)
        {
            pRecord.Set(pQuery);
            return pQuery;
        }

        public static iQueryBuilder Set(this iQueryBuilder pQuery, RecordEntity pEntity)
        {
            return pQuery.Set(pEntity._Record);
        }

        public static iQueryBuilder Set(this iQueryBuilder pQuery, string pField, Guid pGuid)
        {
            QueryBuilder query = (QueryBuilder)pQuery;
            string name = query.getNextParameterName();
            query.Param(name, pGuid.ToByteArray());
            query.Fields().Add(new FieldValue(pField, string.Format("@{0}", name)));
            return query;
        }

        public static iQueryBuilder Set(this iQueryBuilder pQuery, string pField, object pValue)
        {
            QueryBuilder query = (QueryBuilder)pQuery;
            string name = query.getNextParameterName();
            query.Param(name, pValue);
            query.Fields().Add(new FieldValue(pField, string.Format("@{0}", name)));
            return query;
        }

        public static iQueryBuilder SetEx(this iQueryBuilder pQuery, string pField, string pExpression)
        {
            QueryBuilder query = (QueryBuilder)pQuery;
            query.Fields().Add(new FieldValue(pField, pExpression));
            return query;
        }
    }
}