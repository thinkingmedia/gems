namespace DataSources.Query
{
    /// <summary>
    /// Collection of conditions.
    /// </summary>
    public class Conditions : AbstractQueryFeature, iCondition
    {
        /// <summary>
        /// The root for the condition tree.
        /// </summary>
        public readonly Condition Root;

        /// <summary>
        /// The current condition block.
        /// </summary>
        public Condition Current { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Conditions(iQueryBuilder pQuery)
            : base(pQuery)
        {
            Current = Root = new Condition(null, Condition.eOPERATOR.AND);
        }

        /// <summary>
        /// Starts a new condition block that uses AND to join rules.
        /// </summary>
        public iCondition And()
        {
            Condition con = new Condition(Current, Condition.eOPERATOR.AND);
            Current.Add(con);
            Current = con;
            return this;
        }

        /// <summary>
        /// Starts a new condition block that uses OR to join rules.
        /// </summary>
        public iCondition Or()
        {
            Condition con = new Condition(Current, Condition.eOPERATOR.OR);
            Current.Add(con);
            Current = con;
            return this;
        }

        /// <summary>
        /// Closes the current condition block and makes the parent block the current.
        /// </summary>
        public iCondition Up()
        {
            Current = Current.Parent ?? Root;
            return this;
        }

        /// <summary>
        /// The query
        /// </summary>
        public iQueryBuilder End()
        {
            return Query();
        }
    }
}