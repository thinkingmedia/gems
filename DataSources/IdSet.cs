using System;
using System.Collections.Generic;

namespace DataSources
{
    /// <summary>
    /// Holds a collection of record IDs.
    /// </summary>
    public class IdSet : HashSet<uint>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public IdSet()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public IdSet(IEnumerable<uint> pIdSet)
            : base(pIdSet)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public IdSet(List<uint> pIdSet)
            : base(pIdSet)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public IdSet(IEnumerable<Record> pRecords)
            : this()
        {
            foreach (Record record in pRecords)
            {
                Add(record.ID);
            }
        }

        /// <summary>
        /// Converts to a list.
        /// </summary>
        public List<uint> ToList()
        {
            return new List<uint>(this);
        }

        /// <summary>
        /// Converts to an array.
        /// </summary>
        public UInt32[] ToArray()
        {
            return ToList().ToArray();
        }
    }
}
