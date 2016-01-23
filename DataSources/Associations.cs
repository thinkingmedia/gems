using System;
using System.Collections.Generic;
using System.Linq;
using Common.Exceptions;
using DataSources.Attributes;
using DataSources.DataSource;
using DataSources.Exceptions;

namespace DataSources
{
    /// <summary>
    /// Manages the associated Models for a Model.
    /// </summary>
    public sealed class Associations : IDisposable
    {
        /// <summary>
        /// A list of different association types.
        /// </summary>
        private enum AssocType
        {
            /// <summary>
            /// The Model that owns all the associated models.
            /// </summary>
            OWNER,
            /// <summary>
            /// Owner has many of these model records.
            /// </summary>
            HAS_MANY,
            /// <summary>
            /// Owner belongs to this model record.
            /// </summary>
            BELONGS_TO
        }

        /// <summary>
        /// Used to hold a reference to the model.
        /// </summary>
        private class ModelRef
        {
            /// <summary>
            /// The alias for this reference.
            /// </summary>
            private string _alias;

            /// <summary>
            /// The associations type.
            /// </summary>
            private AssocType _type;

            /// <summary>
            /// The model object.
            /// </summary>
            public readonly Model Model;

            /// <summary>
            /// Constructor
            /// </summary>
            public ModelRef(string pAlias, AssocType pAssocType, Model pModel)
            {
                _alias = pAlias;
                _type = pAssocType;
                Model = pModel;
            }
        }

        /// <summary>
        /// Uses to hold the different aliases for a Model
        /// </summary>
        private class Aliases : Dictionary<string, ModelRef>
        {
        }

        /// <summary>
        /// 
        /// </summary>
        private class Models : Dictionary<Type, Aliases>
        {
            new public Aliases this[Type pType]
            {
                get
                {
                    if (!ContainsKey(pType))
                    {
                        base[pType] = new Aliases();
                    }
                    return base[pType];
                }
            }
        }

        /// <summary>
        /// The list of associated models, grouped by Type.
        /// </summary>
        private readonly Models _models;

        /// <summary>
        /// A list of all aliases used (they have to remain unique).
        /// </summary>
        private readonly HashSet<string> _aliases;

        /// <summary>
        /// Accesses an association by it's type, but only if that type is used once.
        /// Otherwise an exception is thrown, and you have to use the alias.
        /// </summary>
        public Model this[Type pType]
        {
            get
            {
                if (_models[pType].Count == 0)
                {
                    throw new ModelException(string.Format("Model {0} is not associated with this model.", pType.Name));
                }
                if (_models[pType].Count == 1)
                {
                    return _models[pType].First().Value.Model;
                }

                throw new ModelException(string.Format("Model {0} has more then one association to this model.", pType.Name));
            }
        }

        /// <summary>
        /// Finds an associated model by it's alias.
        /// </summary>
        public Model this[string pAlias]
        {
            get
            {
                if (!_aliases.Contains(pAlias))
                {
                    throw new ModelException(string.Format("Alias {0} is not associated with this model.", pAlias));
                }
                foreach (var y in _models.SelectMany(pX => pX.Value.Where(pY => pY.Key == pAlias)))
                {
                    return y.Value.Model;
                }

                throw new ModelException(string.Format("Alias {0} is not associated with this model.", pAlias));
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Associations()
        {
            _models = new Models();
            _aliases = new HashSet<string>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Associations(string pFirstAlias, Model pModel)
            : this()
        {
            Attach(pModel.GetType(), pFirstAlias, AssocType.OWNER, pModel);
        }

        /// <summary>
        /// Disposes of the object.
        /// </summary>
        public void Dispose()
        {
            _aliases.Clear();
            _models.Clear();
        }

        /// <summary>
        /// Attaches the model to the owner as an associated model.
        /// </summary>
        private void Attach(Type pType, string pAlias, AssocType pAssocType, Model pModel)
        {
            if (_aliases.Add(pAlias) == false)
            {
                throw new ModelException(string.Format("Alias {0} already used.", pAlias));
            }
            _models[pType][pAlias] = new ModelRef(pAlias, pAssocType, pModel);
        }

        /// <summary>
        /// Attaches a Model as a BelongsTo
        /// </summary>
        /// <param name="pAlias"></param>
        /// <param name="pModel"></param>
        public void AttachBelongsTo(string pAlias, Model pModel)
        {
            Attach(pModel.GetType(), pAlias, AssocType.BELONGS_TO, pModel);
        }

        /// <summary>
        /// Attaches a Model to HasMany
        /// </summary>
        /// <param name="pAlias"></param>
        /// <param name="pModel"></param>
        public void AttachHasMany(string pAlias, Model pModel)
        {
            Attach(pModel.GetType(), pAlias, AssocType.HAS_MANY, pModel);
        }

        /// <summary>
        /// Detaches a Model
        /// </summary>
        public void Detach(string pAlias)
        {
            _aliases.Remove(pAlias);
            if (_models.Any(pX => pX.Value.Remove(pAlias)))
            {
                return;
            }

            throw new ModelException(string.Format("Alias {0} is not attached.", pAlias));
        }

        /// <summary>
        /// Attaches the Models defined as custom attributes in the Model class.
        /// </summary>
        /// <param name="pModel">The model to inspect</param>
        /// <param name="pDataSource"></param>
        public void CreateByAttributes(Model pModel, iDataSource pDataSource)
        {
            object[] attributes = pModel.GetType().GetCustomAttributes(typeof(HasMany), true);
            foreach (object attribute in attributes)
            {
                HasMany hm = attribute as HasMany;
                Model m = ModelRegistry.init(hm.ModelType, pDataSource);
                AttachHasMany(hm.Alias, m);
            }
        }
    }
}
