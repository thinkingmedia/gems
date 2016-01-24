using System.Collections;
using System.Collections.Generic;
using DataSources.Query;
using GemsLogger;

namespace DataSources
{
    /// <summary>
    /// Reads records from a query in chunks and provides an enumerator interface so that
    /// it works with foreach loops.
    /// </summary>
    public class QueryCollection : IEnumerator<Record>, IEnumerable<Record>
    {
        /// <summary>
        /// Logging
        /// </summary>
        private static readonly Logger _logger = Logger.Create(typeof(QueryCollection));

        /// <summary>
        /// The record count.
        /// </summary>
        private readonly int _size;

        /// <summary>
        /// The current collection of records.
        /// </summary>
        private HashSet<Record>.Enumerator _cursor;

        /// <summary>
        /// The current offset
        /// </summary>
        private int _offset;

        /// <summary>
        /// The query.
        /// </summary>
        private iQueryBuilder _query;

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Reads the next set of records.
        /// </summary>
        private void ReadMore()
        {
            Records records = _query
                .Limit(_size)
                .Offset(_offset)
                .All();

            _cursor = records.GetEnumerator();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pQuery">The query that will be run to collect records.</param>
        /// <param name="pSize">The number of records to read at a time.</param>
        public QueryCollection(iQueryBuilder pQuery, int pSize)
        {
            _query = pQuery;
            _size = pSize;

            Reset();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Record> GetEnumerator()
        {
            return this;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _query = null;
        }

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        public bool MoveNext()
        {
            if (_cursor.MoveNext())
            {
                return true;
            }

            _offset += _size;
            ReadMore();

            return _cursor.MoveNext();
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        public void Reset()
        {
            _offset = 0;
            ReadMore();
        }

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        public Record Current
        {
            get { return _cursor.Current; }
        }

        /// <summary>
        /// Gets the current element in the collection.
        /// </summary>
        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}