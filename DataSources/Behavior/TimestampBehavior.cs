using System;
using DataSources.Query;

namespace DataSources.Behavior
{
    /// <summary>
    /// Handles the automatic settings of `Created` and `Updated` fields during
    /// create and update operations.
    /// </summary>
    public class TimestampBehavior : Behavior
    {
        /// <summary>
        /// The field to set a time stamp.
        /// </summary>
        private readonly string _fieldName;

        /// <summary>
        /// Timestamp on create.
        /// </summary>
        private readonly bool _onCreate;

        /// <summary>
        /// Timestamp on update.
        /// </summary>
        private readonly bool _onUpdate;

        /// <summary>
        /// Adds a timestamp to a field.
        /// </summary>
        private static void Stamp(iQueryBuilder pQuery, string pFieldname)
        {
            if (pQuery.Model().HasField(pFieldname) 
                && !pQuery.Fields().HasField(pFieldname))
            {
                pQuery.SetEx(pFieldname, "NOW()");
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TimestampBehavior(string pFieldName, bool pOnCreate, bool pOnUpdate)
        {
            _fieldName = pFieldName;
            _onCreate = pOnCreate;
            _onUpdate = pOnUpdate;
        }

        /// <summary>
        /// Adds the time stamp when creating records.
        /// </summary>
        public override bool BeforeInsert(QueryBuilder pQuery)
        {
            if (_onCreate)
            {
                Stamp(pQuery, _fieldName);
            }
            
            // add the timestamp to the ON DUPLICATE KEY part of the query
            if (!_onUpdate || !pQuery.OnDuplicateEnabled)
            {
                return true;
            }

            pQuery.OnDuplicate();
            Stamp(pQuery, _fieldName);
            pQuery.OnInsert();
            return true;
        }

        /// <summary>
        /// Adds the time stamp when updating records.
        /// </summary>
        public override bool BeforeUpdate(QueryBuilder pQuery)
        {
            if (_onUpdate)
            {
                Stamp(pQuery, _fieldName);
            }
            return true;
        }
    }
}