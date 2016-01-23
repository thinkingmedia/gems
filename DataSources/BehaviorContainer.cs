using System;
using System.Collections.Generic;
using System.Linq;
using DataSources.Exceptions;
using DataSources.Query;

namespace DataSources
{
    /// <summary>
    /// A class used to manage attached behaviors for a model.
    /// </summary>
    public sealed class BehaviorContainer : iModelEvents
    {
        /// <summary>
        /// A list of behaviors used by this model.
        /// </summary>
        private readonly List<Behavior.Behavior> _behaviors;

        /// <summary>
        /// A list of objects to receive callbacks.
        /// </summary>
        private readonly List<iModelEvents> _callBacks;

        /// <summary>
        /// The model to be associated with the behaviors of this
        /// container.
        /// </summary>
        private readonly Model _model;

        /// <summary>
        /// Constructor
        /// </summary>
        public BehaviorContainer(Model pModel)
        {
            _model = pModel;
            _behaviors = new List<Behavior.Behavior>();
            _callBacks = new List<iModelEvents>();
        }

        /// <summary>
        /// Adds a behavior to the container.
        /// </summary>
        public Behavior.Behavior Add(Behavior.Behavior pBehavior)
        {
            _behaviors.Add(pBehavior);
            RegisterEventListener(pBehavior);
            pBehavior.AttachModel(_model);
            return pBehavior;
        }

        /// <summary>
        /// Access the FIRST behavior by it's type. If the behavior is not
        /// attached then a BehaviorException is thrown.
        /// </summary>
        /// <typeparam name="T">The behavior type</typeparam>
        /// <returns>The behavior object</returns>
        /// <exception cref="BehaviorException">If the behavior is not attached.</exception>
        public T Get<T>() where T : Behavior.Behavior
        {
            Type type = typeof (T);
            Behavior.Behavior obj = (from behavior in _behaviors where type == behavior.GetType() select behavior).FirstOrDefault();
            if (obj == null)
            {
                throw new BehaviorException("Behavior not attached. {0} doesn't have behavior {1}", _model.Settings.Alias, type.FullName );
            }
            return obj as T;
        }

        /// <summary>
        /// Adds a callback event handler to the model.
        /// </summary>
        /// <param name="pEvents">The object to receive notifications.</param>
        public void RegisterEventListener(iModelEvents pEvents)
        {
            if (!_callBacks.Contains(pEvents))
            {
                _callBacks.Add(pEvents);
            }
        }

        /// <summary>
        /// Removes a callback event handler.
        /// </summary>
        /// <param name="pEvents">The object to unregistered.</param>
        public void UnregisterEventListener(iModelEvents pEvents)
        {
            _callBacks.Remove(pEvents);
        }

        /// <summary>
        /// Called before a find operation.
        /// </summary>
        public void BeforeSelect(QueryBuilder pQuery)
        {
            _callBacks.ForEach(pCallback => pCallback.BeforeSelect(pQuery));
        }

        /// <summary>
        /// Dispatches to all listeners.
        /// </summary>
        /// <param name="pRecords"></param>
        /// <returns></returns>
        public Records AfterSelect(Records pRecords)
        {
            return _callBacks.Aggregate(pRecords, (pCurrent, pCallback) => pCallback.AfterSelect(pCurrent));
        }

        /// <summary>
        /// Place any pre-save logic in this callback. To abort saving of the
        /// record return False.
        /// </summary>
        /// <returns>True to save the record.</returns>
        public bool BeforeUpdate(QueryBuilder pQuery)
        {
            return _callBacks.Aggregate(true, (pCurrent, pCallback) => pCurrent & pCallback.BeforeUpdate(pQuery));
        }

        /// <summary>
        /// Dispatches to all listeners.
        /// </summary>
        public void AfterUpdate()
        {
            _callBacks.ForEach(pCallback=>pCallback.AfterUpdate());
        }

        /// <summary>
        /// Place any pre-deletion logic here.
        /// </summary>
        /// <returns>True to delete the record.</returns>
        public bool BeforeDelete(QueryBuilder pQuery)
        {
            return _callBacks.Aggregate(true, (pCurrent, pCallback) => pCurrent & pCallback.BeforeDelete(pQuery));
        }

        /// <summary>
        /// Dispatches to all listeners.
        /// </summary>
        public void AfterDelete()
        {
            _callBacks.ForEach(pCallback=>pCallback.AfterDelete());
        }

        /// <summary>
        /// Place any pre-create logic in this callback. To abort creating of the
        /// record return false.
        /// </summary>
        /// <returns>True to create the record.</returns>
        public bool BeforeInsert(QueryBuilder pQuery)
        {
            return _callBacks.Aggregate(true, (pCurrent, pCallback) => pCurrent & pCallback.BeforeInsert(pQuery));
        }

        /// <summary>
        /// Dispatches to all listeners.
        /// </summary>
        public void AfterInsert()
        {
            _callBacks.ForEach(pCallback=>pCallback.AfterInsert());
        }
    }
}
