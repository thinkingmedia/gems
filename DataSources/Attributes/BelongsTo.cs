using System;

namespace DataSources.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class BelongsTo : System.Attribute
    {
        private readonly Type _type;

        public BelongsTo(Type pType)
        {
            _type = pType;
        }
    }
}
