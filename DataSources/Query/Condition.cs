using System.Collections.Generic;
using System.Linq;

namespace DataSources.Query
{
    /// <summary>
    /// </summary>
    public class Condition
    {
        /// <summary>
        /// Different type of join operators.
        /// </summary>
        public enum eOPERATOR
        {
            AND,
            OR
        }

        /// <summary>
        /// The parent block for this block.
        /// </summary>
        public readonly Condition Parent;

        /// <summary>
        /// A list of conditions
        /// </summary>
        private readonly List<Entry> _conditions;

        /// <summary>
        /// How conditions in this list of join together
        /// </summary>
        private readonly eOPERATOR _operator;

        /// <summary>
        /// Constructor
        /// </summary>
        public Condition(Condition pParent, eOPERATOR pOperator)
        {
            Parent = pParent;
            _operator = pOperator;
            _conditions = new List<Entry>();
        }

        /// <summary>
        /// Adds the expression.
        /// </summary>
        public Condition Add(string pExpression)
        {
            _conditions.Add(new Entry {Expression = pExpression});
            return this;
        }

        /// <summary>
        /// Adds an inner condition.
        /// </summary>
        public Condition Add(Condition pCondition)
        {
            _conditions.Add(new Entry {Inner = pCondition});
            return this;
        }

        /// <summary>
        /// Renders the conditions to a SQL where clause.
        /// </summary>
        /// <returns>The SQL where statement.</returns>
        public string Render()
        {
            return string.Join(
                string.Format(" {0} ", _operator),
                from entry in _conditions select string.Format("({0})", entry)
                );
        }

        /// <summary>
        /// Holds the value of a condition entry.
        /// </summary>
        private struct Entry
        {
            public string Expression;
            public Condition Inner;

            public override string ToString()
            {
                return Expression ?? Inner.Render();
            }
        }
    }
}