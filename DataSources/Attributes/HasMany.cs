using System;

namespace DataSources.Attributes
{
    /// <summary>
    /// Represents the association of a HasMany to the model.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class HasMany : System.Attribute
    {
        /// <summary>
        /// The model class to associate.
        /// </summary>
        public readonly Type ModelType;

        /// <summary>
        /// The alias for the model.
        /// </summary>
        public readonly string Alias;

        /// <summary>
        /// Constructor
        /// </summary>
        public HasMany(Type pModelType)
            : this(pModelType, pModelType.Name)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HasMany(Type pModelType, string pAlias)
        {
            ModelType = pModelType;
            Alias = pAlias;
        }
    }
}
