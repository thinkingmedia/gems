using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataSources.Query
{
    public static class QueryGroup
    {
        public static iQueryBuilder Group(this iQueryBuilder pQuery, IEnumerable<string> pFields)
        {
            if (pFields == null)
            {
                return pQuery;
            }
            ((QueryBuilder)pQuery).Groups().Add(from str in pFields select new FieldValue(str));
            return pQuery;
        }

        public static iQueryBuilder Group(this iQueryBuilder pQuery, params string[] pFields)
        {
            return Group(pQuery, (IEnumerable<string>)pFields);
        }
    }
}
