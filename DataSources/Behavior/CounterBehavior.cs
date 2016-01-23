using System;
using DataSources.Query;

namespace DataSources.Behavior
{
    /// <summary>
    /// Handles increment a field value.
    /// </summary>
    public class CounterBehavior : Behavior
    {
        /// <summary>
        /// Decrease the value of a field by one. Unless the field value is already zero.
        /// </summary>
        /// <param name="pID">The record ID</param>
        /// <param name="pField">The name of the field.</param>
        /// <param name="pAmount"></param>
        public void Decrement(UInt32 pID, string pField, int pAmount = 1)
        {
            string exp = string.Format("{0} - {1}", Model.Field(pField), pAmount);
            Model.Update()
                .SetEx(pField, exp)
                .Where()
                .Op(pField, ">", 0)
                .isID(pID)
                .End()
                .Execute();
        }

        /// <summary>
        /// Increase the value of a field by one.
        /// </summary>
        /// <param name="pID">The record ID</param>
        /// <param name="pField">The name of the field.</param>
        /// <param name="pAmount">The increase amount.</param>
        public void Increment(UInt32 pID, string pField, int pAmount = 1)
        {
            string exp = string.Format("{0} + {1}", Model.Field(pField), pAmount);

            Model.Update()
                .SetEx(pField, exp)
                .Where()
                .isID(pID)
                .End()
                .Execute();
        }
    }
}