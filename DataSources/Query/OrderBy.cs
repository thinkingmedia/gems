using System;
using System.Collections.Generic;
using System.Linq;
using DataSources.Exceptions;

namespace DataSources.Query
{
    /// <summary>
    /// Stores the ORDER BY fields
    /// </summary>
    [Obsolete]
    public class OrderBy : Dictionary<string, string>
    {
        /// <summary>
        /// Adds a ORDER BY string
        /// </summary>
        /// <param name="pDict"></param>
        /// <param name="pValue"></param>
        private static Dictionary<string, string> CreateOrderBy(Dictionary<string, string> pDict, string pValue)
        {
            string[] orders = pValue.Split(new[] {','});
            foreach (string order in orders)
            {
                string[] parts = order.Trim().Split(new[] {' '});
                string key, value;
                switch (parts.Length)
                {
                    case 2:
                        key = parts[0].Trim();
                        value = parts[1].Trim().ToUpper();
                        break;
                    case 1:
                        key = parts[0].Trim();
                        value = "ASC";
                        break;
                    default:
                        throw new OrderByException("Syntax error in order by clause: {0}", pValue);
                }
                if (pDict.ContainsKey(key))
                {
                    throw new OrderByException("Order by field already contains a clause: {0}", pValue);
                }
                pDict[key] = value;
            }
            return pDict;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderBy()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderBy(string pFieldname)
            : base(CreateOrderBy(new Dictionary<string, string>(), pFieldname))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pValues"></param>
        public OrderBy(IDictionary<string, string> pValues)
            : base(pValues)
        {
        }

        /// <summary>
        /// Converts to a SQL ORDER BY statement
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Join(",", from pair in this select string.Format("{0} {1}", pair.Key, pair.Value));
        }

        /// <summary>
        /// Adds a ORDER BY string
        /// </summary>
        /// <param name="pValue"></param>
        public void add(string pValue)
        {
            CreateOrderBy(this, pValue);
        }
    }
}