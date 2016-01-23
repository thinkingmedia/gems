using System;
using System.Collections.Generic;

namespace DataSources.Query
{
    /// <summary>
    /// Holds query parameters for Find operations in the model.
    /// </summary>
    // ReSharper disable InconsistentNaming
    [Obsolete]
    public class QueryData
    {
        /// <summary>
        /// The conditions for the query. Join by AND.
        /// </summary>
        public List<string> conditions { get; private set; }

        /// <summary>
        /// The fields for the query.
        /// </summary>
        public List<string> fields { get; private set; }

        /// <summary>
        /// The fields to group by.
        /// </summary>
        public GroupBy groupBy { get; private set; }

        /// <summary>
        /// The limit (zero is no limit)
        /// </summary>
        public int limit { get; set; }

        /// <summary>
        /// The fields to order by and the direction.
        /// </summary>
        public OrderBy orderBy { get; private set; }

        /// <summary>
        /// If there is a limit this is the page to start on.
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public QueryData()
        {
            fields = new List<string>();
            conditions = new List<string>();
            groupBy = new GroupBy();
            orderBy = new OrderBy();
        }

        /// <summary>
        /// Constructor with simple parameters.
        /// </summary>
        public QueryData(IEnumerable<string> Fields = null, string Conditions = null, string GroupBy = null,
                         string OrderBy = null, int Limit = 0, int Page = 0)
            : this()
        {
            if (Fields != null)
            {
                fields.AddRange(Fields);
            }

            if (Conditions != null)
            {
                conditions.Add(Conditions);
            }

            if (GroupBy != null)
            {
                groupBy.AddRange(GroupBy.Split(new[] {','}));
            }

            if (OrderBy != null)
            {
                orderBy.add(OrderBy);
            }

            limit = Limit;
            page = Page;
        }

        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        public QueryData(IEnumerable<string> Fields = null, IEnumerable<string> Conditions = null,
                         IEnumerable<string> GroupBy = null, IDictionary<string, string> OrderBy = null, int Limit = 0,
                         int Page = 0)
            : this()
        {
            if (Fields != null)
            {
                fields.AddRange(Fields);
            }

            if (Conditions != null)
            {
                conditions.AddRange(Conditions);
            }

            if (GroupBy != null)
            {
                groupBy.AddRange(GroupBy);
            }

            if (OrderBy != null)
            {
                orderBy = new OrderBy(OrderBy);
            }

            limit = Limit;
            page = Page;
        }
    }
}