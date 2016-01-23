using DataSources.Query;

namespace DataSources
{
    /// <summary>
    /// Provides a default implementation of the model events and makes it easy to
    /// maintain the interface.
    /// </summary>
    public abstract class DefaultModelEvents : iModelEvents
    {
        /// <summary>
        /// Called before a find operation.
        /// </summary>
        public virtual void BeforeSelect(QueryBuilder pQuery)
        {
        }

        /// <summary>
        /// Default implementation.
        /// </summary>
        public virtual Records AfterSelect(Records pRecords)
        {
            return pRecords;
        }

        /// <summary>
        /// Place any pre-save logic in this callback. To abort saving of the
        /// record return False.
        /// </summary>
        /// <returns>True to save the record.</returns>
        public virtual bool BeforeUpdate(QueryBuilder pQuery)
        {
            return true;
        }

        /// <summary>
        /// Default implementation.
        /// </summary>
        public virtual void AfterUpdate()
        {
        }

        /// <summary>
        /// Place any pre-deletion logic here.
        /// </summary>
        /// <returns>True to delete the record.</returns>
        public virtual bool BeforeDelete(QueryBuilder pQuery)
        {
            return true;
        }

        /// <summary>
        /// Default implementation.
        /// </summary>
        public virtual void AfterDelete()
        {
        }

        /// <summary>
        /// Place any pre-create logic in this callback. To abort creating of the
        /// record return false.
        /// </summary>
        /// <returns>True to create the record.</returns>
        public virtual bool BeforeInsert(QueryBuilder pQuery)
        {
            return true;
        }

        /// <summary>
        /// Default implementation.
        /// </summary>
        public virtual void AfterInsert()
        {
        }
    }
}