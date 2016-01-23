using System;
using DataSources.Exceptions;
using DataSources.Query;

namespace DataSources.Behavior
{
    /// <summary>
    /// Provides a timeout behavior for records.
    /// 
    /// Supports simple timeouts when the `delta` field is set to false, and
    /// progressive timeouts when set to an integer field.
    /// 
    /// Progressive timeouts grow longer each time the record is delayed again,
    /// until the max timeout is reached.
    /// </summary>
    public class DelayBehavior : Behavior
    {
        /// <summary>
        /// Name of the next DateTime column
        /// </summary>
        public readonly string NextField;

        /// <summary>
        /// Name of the delta int column.
        /// </summary>
        public readonly string DeltaField;

        /// <summary>
        /// Constructor
        /// </summary>
        public DelayBehavior(string pNextField = "next", string pDeltaField = "delta")
        {
            NextField = pNextField;
            DeltaField = pDeltaField;
        }

        /// <summary>
        /// Sets the defaults for new records.
        /// </summary>
        public override bool BeforeInsert(QueryBuilder pQuery)
        {
            if (!Model.Data.Has(DeltaField))
            {
                Model.Data[DeltaField] = 0;
            }

            return base.BeforeInsert(pQuery);
        }

        /// <summary>
        /// Increases the delay for the current record by 1 delta. 
        /// 
        /// Marks a record as having been processed and should not be worked
        /// on again for the configured interval. The delta is incremented,
        /// and the next field is set for when it should be worked on again.
        /// </summary>
        public void NoChange(UInt32 pID)
        {
            const int interval = 15;
            const int max = 60;
            const int maxDelta = 10;

            const string unit = "MINUTE";
            string formula = string.Format("NOW() + INTERVAL LEAST({0} * POW(2,`{3}`-1),{1}) {2}", interval, max, unit, DeltaField);

            Model.Update()
                .SetEx(DeltaField, string.Format("LEAST(`{0}`+1,{1})", DeltaField, maxDelta))
                .SetEx(NextField, formula)
                .Where()
                .isID(pID)
                .End()
                .Execute();
        }

        /// <summary>
        /// Marks a record to be delayed just 1 interval. The record
        /// will not be found again by `FindNext` until 1 interval has
        /// expired.
        /// </summary>
        public void Changed(UInt32 pID)
        {
            const int interval = 15;
            const string unit = "MINUTE";
            string forumla = string.Format("NOW() + INTERVAL {0} {1}", interval, unit);

            Model.Update()
                .Set(DeltaField, 1)
                .SetEx(NextField, forumla)
                .Where()
                .isID(pID)
                .End()
                .Execute();
        }

        /// <summary>
        /// Clears the delay properties of a record.
        /// </summary>
        public void Clear(UInt32 pID)
        {
            Model.Update()
                .Set(DeltaField, 0)
                .Set(NextField, null)
                .Where()
                .isID(pID)
                .End()
                .Execute();
        }

        /// <summary>
        /// Clears the delay properties of all the records.
        /// </summary>
        public void ClearAll()
        {
            Model.Update()
                .Set(DeltaField, 0)
                .Set(NextField, null)
                .Execute();
        }
    }
}
