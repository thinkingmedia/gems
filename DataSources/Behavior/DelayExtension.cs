using DataSources.Query;

namespace DataSources.Behavior
{
    public static class DelayExtension
    {
        /// <summary>
        /// Finds all the records that have had their delay expired.
        /// </summary>
        public static iCondition isDelayExpired(this iCondition pCon)
        {
            DelayBehavior behavior = pCon.Model().Behaviors.Get<DelayBehavior>();
            return pCon
                .Or()
                .Expression(behavior.NextField, "<= NOW()")
                .isNull(behavior.NextField)
                .Up();
        }
    }
}