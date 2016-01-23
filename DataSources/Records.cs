using System;
using System.Collections.Generic;
using System.Linq;
using Common.Exceptions;
using DataSources.Exceptions;

namespace DataSources
{
    /// <summary>
    /// Holds a collection of records.
    /// </summary>
    public class Records : HashSet<Record>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Records()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Records(IEnumerable<Record> pRecords)
            : base(pRecords)
        {
        }

        /// <summary>
        /// Converts to a list.
        /// </summary>
        public List<Record> ToList()
        {
            return new List<Record>(this);
        }

        /// <summary>
        /// Converts to an array.
        /// </summary>
        public Record[] ToArray()
        {
            return ToList().ToArray();
        }

        /// <summary>
        /// A list of record IDs.
        /// </summary>
        public IdSet ToIdSet()
        {
            return new IdSet(this);
        }

        /// <summary>
        /// Creates a record ID set from a fieldname in the collection.
        /// </summary>
        public IdSet ToIdSet(string pFieldname)
        {
            IdSet set = new IdSet();
            foreach (Record record in this)
            {
                if (record.isNull(pFieldname))
                {
                    throw new ModelException(string.Format("Can not add field {0} to ID set, because field is null.", pFieldname));
                }
                set.Add((UInt32)record.getUInt32(pFieldname));
            }
            return set;
        }
    }
}
