using System.Collections.Generic;
using System.Linq;

namespace DataSources.Query
{
    /// <summary>
    /// Defines a table that is used in a JOIN query.
    /// </summary>
    public class JoinTable
    {
        /// <summary>
        /// The fields for this join.
        /// </summary>
        public readonly Fields Fields;

        /// <summary>
        /// The table to join.
        /// </summary>
        public readonly string Table;

        /// <summary>
        /// Constructor
        /// </summary>
        public JoinTable(iQueryBuilder pQuery, string pTable, IEnumerable<string> pFields)
        {
            Table = pTable;
            Fields = new Fields(pQuery);

            if (pFields == null)
            {
                return;
            }

            pFields.ToList().ForEach(pStr=>Fields.Add(new FieldValue(pStr)));
        }
    }
}