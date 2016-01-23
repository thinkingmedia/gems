namespace DataSources.Query
{
    public interface iCondition : iParameters
    {
        /// <summary>
        /// Starts a new condition block that uses AND to join rules.
        /// </summary>
        iCondition And();

        /// <summary>
        /// Starts a new condition block that uses OR to join rules.
        /// </summary>
        iCondition Or();

        /// <summary>
        /// Returns the query builder to continue the chain.
        /// </summary>
        iQueryBuilder End();

        /// <summary>
        /// Closes the current condition block and makes the parent block the current.
        /// </summary>
        /// <returns></returns>
        iCondition Up();
    }
}