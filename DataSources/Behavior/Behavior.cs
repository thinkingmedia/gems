using System;
using Common.Exceptions;
using DataSources.Exceptions;

namespace DataSources.Behavior
{
    /// <summary>
    /// The base class used trails
    /// </summary>
    public abstract class Behavior : DefaultModelEvents, IDisposable
    {
        /// <summary>
        /// The model reference.
        /// </summary>
        private Model _model;

        /// <summary>
        /// The Model used by this Behavior.
        /// </summary>
        protected Model Model
        {
            get
            {
                if (_model == null)
                {
                    throw new BehaviorException("Model has not been attached to behavior.");
                }
                return _model;
            }
        }

        /// <summary>
        /// Release the model reference to help GC.
        /// </summary>
        virtual public void Dispose()
        {
            DetachModel();
        }

        /// <summary>
        /// Attaches this behavior to a model.
        /// </summary>
        /// <param name="pModel">The model to use.</param>
        public void AttachModel(Model pModel)
        {
            _model = pModel;
        }

        /// <summary>
        /// Detaches the model from the behavior.
        /// </summary>
        public void DetachModel()
        {
            _model = null;
        }
    }
}
