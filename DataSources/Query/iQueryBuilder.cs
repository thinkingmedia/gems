namespace DataSources.Query
{

    public interface iQueryBuilder : iParameters, iJoinable
    {
        /// <summary>
        /// The fields for the query.
        /// </summary>
        iFields Fields();

        /// <summary>
        /// The GROUP BY fields for the query.
        /// </summary>
        iFields Groups();

        /// <summary>
        /// Starts a WHERE condition chain.
        /// </summary>
        /// <returns></returns>
        iCondition Where();

        /// <summary>
        /// The type of query.
        /// </summary>
        /// <returns></returns>
        QueryBuilder.eTYPE Type();

        /// <summary>
        /// Used with data sources that allow the current query to write
        /// lock rows examined by the query until the current transaction
        /// is committed.
        /// </summary>
        iQueryBuilder Lock();

        /// <summary>
        /// Enable on duplicate key update.
        /// </summary>
        iQueryBuilder OnDuplicate();

        /// <summary>
        /// Enables setting fields for the INSERT part of the query.
        /// Call this if you're not sure if OnDuplicate was called.
        /// </summary>
        /// <returns></returns>
        iQueryBuilder OnInsert();
    }
}