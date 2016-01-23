using DataSources.Query;

namespace DataSources.Behavior
{
    public static class ActiveExtension
    {
        public static iCondition isActive(this iCondition pCon)
        {
            ActiveBehavior behavior = pCon.Model().Behaviors.Get<ActiveBehavior>();
            return pCon.isTrue(behavior.FieldName);
        }

        public static iCondition isNotActive(this iCondition pCon)
        {
            ActiveBehavior behavior = pCon.Model().Behaviors.Get<ActiveBehavior>();
            return pCon.isFalse(behavior.FieldName);
        }
    }
}