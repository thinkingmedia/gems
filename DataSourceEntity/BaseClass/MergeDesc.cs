using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataSourceEntity.Exceptions;

namespace DataSourceEntity.BaseClass
{
    /// <summary>
    /// Describes a table merge
    /// </summary>
    public class MergeDesc
    {
        /// <summary>
        /// The name of the base class
        /// </summary>
        public readonly string ClassName;

        /// <summary>
        /// A list of table to be merged.
        /// </summary>
        public readonly List<string> Tables;

        /// <summary>
        /// Constructor
        /// </summary>
        public MergeDesc(string pClassName, IEnumerable<string> pTables)
        {
            ClassName = pClassName;
            Tables = new List<string>(pTables);
            if (Tables.Any(string.IsNullOrWhiteSpace))
            {
                throw new MergeException("Invalid table name for merge feature. Possible double + used.");
            }
            if (string.IsNullOrWhiteSpace(ClassName))
            {
                throw new MergeException("Invalid classname for merge feature.");
            }
        }
    }
}
