using System;
using System.Collections.Generic;

namespace DataSources.Query
{
    /// <summary>
    /// Stores GROUP BY fields as a list.
    /// </summary>
    [Obsolete]
    public class GroupBy : List<string>
    {
        /// <summary>
        /// Convert to a SQL WHERE clause.
        /// </summary>
        /// <returns>The conditions.</returns>
        public override string ToString()
        {
            return string.Join(", ", this);
        }
    }
}