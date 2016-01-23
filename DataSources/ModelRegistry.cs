using System;
using System.Runtime.CompilerServices;
using Common.Exceptions;
using Common.Utils;
using DataSources.DataSource;
using DataSources.Exceptions;

namespace DataSources
{
    /// <summary>
    /// Handles the storing of Model instances, and auto creating Models.
    /// </summary>
    [Obsolete]
    public sealed class ModelRegistry : Singleton<ModelRegistry>
    {
        /// <summary>
        /// Gets a reference to a model.
        /// </summary>
        public static T init<T>(iDataSource pSource) where T : Model
        {
            Type type = typeof(T);
            return (T)Instance.create(type.Name, type, pSource);
        }

        /// <summary>
        /// Gets a reference to a model.
        /// </summary>
        public static Model init(Type pType, iDataSource pSource)
        {
            return Instance.create(pType.Name, pType, pSource);
        }

        /// <summary>
        /// Registers a callback used to create the model.
        /// </summary>
        /// <param name="pCreator"></param>
        public static void register<TClass>(Creators<string, Model>.CreateFunc pCreator) where TClass : Model
        {
            Instance._creators.Register(typeof(TClass).Name, pCreator);
        }

        /// <summary>
        /// Handles creation of model objects.
        /// </summary>
        private readonly Creators<string, Model> _creators;

        /// <summary>
        /// Constructor
        /// </summary>
        public ModelRegistry()
        {
            _creators = new Creators<string, Model>();
        }

        /// <summary>
        /// Finds an existing Model reference by the same alias, or creates a new instance.
        /// </summary>
        /// <param name="pAlias"></param>
        /// <param name="pType"></param>
        /// <param name="pSource"></param>
        /// <exception cref="ModelException"></exception>
        /// <returns></returns>
        private Model create(string pAlias, Type pType, iDataSource pSource)
        {
            // force C# to load the class so that the static constructor
            // registers it's creator
            RuntimeHelpers.RunClassConstructor(pType.TypeHandle);

            Model model = _creators.Create(pAlias);
            if (model == null)
            {
                throw new ModelException(string.Format("Creation of {0} failed.", pType));
            }

            // Now that the model is in the cache. It can be initialized.
            //model.Initialize(pSource);

            return model;
        }
    }
}
