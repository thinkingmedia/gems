using System;
using DataSources.Query;

namespace DataSources.Behavior
{
    /// <summary>
    /// Common behaviors related to records that contain an active column for
    /// toggling an enabled/disabled state.
    /// </summary>
    public class ActiveBehavior : Behavior
    {
        /// <summary>
        /// Name of the column
        /// </summary>
        public readonly string FieldName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pFieldname">Name of the active column.</param>
        public ActiveBehavior(string pFieldname = "active")
        {
            FieldName = pFieldname;
        }

        /// <summary>
        /// Deactivates a record.
        /// </summary>
        public void Deactivate(UInt32 pId)
        {
            Model.Update()
                .Set(FieldName, 0)
                .Where()
                .isID(pId)
                .End()
                .One();
        }

        /// <summary>
        /// Activates a record.
        /// </summary>
        public void Activate(UInt32 pId)
        {
            Model.Update()
                .Set(FieldName, 1)
                .Where()
                .isID(pId)
                .End()
                .One();
        }

        /// <summary>
        /// Checks if a record is active.
        /// </summary>
        public bool isActive(UInt32 pId)
        {
            return Model.ReadField<bool>(pId,FieldName);
        }
    }
}