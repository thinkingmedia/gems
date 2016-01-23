using DataSources.Query;

namespace DataSources.Behavior
{
    public static class KeyExntension
    {
        public static iCondition isKey(this iCondition pCon, string pKey)
        {
            KeyBehavior behavior = pCon.Model().Behaviors.Get<KeyBehavior>();
            return pCon.Eq(behavior.Key, pKey);
        }
    }
}