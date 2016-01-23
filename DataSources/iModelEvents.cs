using System;
using DataSources.Query;

namespace DataSources
{
    /// <summary>
    /// A list of events that are triggered by a model.
    /// </summary>
    public interface iModelEvents
    {
        /// <summary>
        /// Called before a find operation.
        /// </summary>
        void BeforeSelect(QueryBuilder pQuery);

        /// <summary>
        /// Called after a find operation.
        /// </summary>
        /// <param name="pRecords">A list of records.</param>
        /// <returns>The list of records to use.</returns>
        Records AfterSelect(Records pRecords);

        /// <summary>
        /// Place any pre-save logic in this callback. To abort saving of the
        /// record return False.
        /// </summary>
        /// <returns>True to save the record.</returns>
        bool BeforeUpdate(QueryBuilder pQuery);

        /// <summary>
        /// Place after save logic in this callback.
        /// </summary>
        void AfterUpdate();

        /// <summary>
        /// Place any pre-create logic in this callback. To abort creating of the
        /// record return false.
        /// </summary>
        /// <returns>True to create the record.</returns>
        bool BeforeInsert(QueryBuilder pQuery);

        /// <summary>
        /// Place after create logic in this callback.
        /// </summary>
        void AfterInsert();

        /// <summary>
        /// Place any pre-deletion logic here.
        /// </summary>
        /// <returns>True to delete the record.</returns>
        bool BeforeDelete(QueryBuilder pQuery);

        /// <summary>
        /// Place any post delete logic here.
        /// </summary>
        void AfterDelete();
    }
}
