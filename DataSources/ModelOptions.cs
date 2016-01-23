using System;
using Common.Events;
using DataSources.Query;

namespace DataSources
{
    /// <summary>
    /// Used to pass options to model when it's created.
    /// </summary>
    public class ModelOptions
    {
        /// <summary>
        /// The Alias for this model.
        /// </summary>
        public string Alias;

        /// <summary>
        /// The name of the table to use.
        /// </summary>
        public string Table;

        /// <summary>
        /// The name of the primary field.
        /// </summary>
        public string PrimaryKey = "id";

        /// <summary>
        /// The Type used for the primary key.
        /// </summary>
        public Type PrimaryType = typeof(UInt32);

        /// <summary>
        /// Which field is used to generate `list` results.
        /// </summary>
        public string DisplayField = "title";

        /// <summary>
        /// Initializes the options before they are used by a model.
        /// </summary>
        public void Initialize(Type pType)
        {
            Alias = Alias ?? pType.Name;
            Table = Table ?? Inflector.Tableize(Alias);
        }
    }
}
