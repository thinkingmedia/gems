using DataSources.Query;

namespace DataSources.Behavior
{
    public static class PublishedExtension
    {
        public static iCondition isNotPublished(this iCondition pQuery)
        {
            PublishedBehavior behavior = pQuery.Model().Behaviors.Get<PublishedBehavior>();
            return pQuery.isFalse(behavior.FieldName);
        }

        public static iCondition isPublished(this iCondition pQuery)
        {
            PublishedBehavior behavior = pQuery.Model().Behaviors.Get<PublishedBehavior>();
            return pQuery.isTrue(behavior.FieldName);
        }
    }
}