using System;
using System.Collections.Generic;

namespace Common.Utils
{
    /// <summary>
    /// Manages a collection of callback functions used to create instances of
    /// an object. The callbacks can be grouped by TKey.
    /// </summary>
    public class Creators<TKey, TClass>
    {
        /// <summary>
        /// The exception thrown for errors.
        /// </summary>
        private class CreatorException : Exception
        {
            public CreatorException(string pMsg)
                : base(pMsg)
            {
            }
        }

        /// <summary>
        /// A callback method for creating instances of a model.
        /// </summary>
        public delegate TClass CreateFunc();

        /// <summary>
        /// A collection of factory callbacks for creating TClass.
        /// </summary>
        private readonly Dictionary<TKey, Creators<TKey, TClass>.CreateFunc> _creators;

        /// <summary>
        /// Constructor
        /// </summary>
        public Creators()
        {
            _creators = new Dictionary<TKey, CreateFunc>();
        }

        /// <summary>
        /// Registers a creator for a key.
        /// </summary>
        public void Register(TKey pKey, CreateFunc pCreator)
        {
            lock (_creators)
            {
                if (_creators.ContainsKey(pKey))
                {
                    throw new CreatorException(string.Format("Creator already registered for [{0}]", pKey));
                }
                _creators.Add(pKey, pCreator);
            }
        }

        /// <summary>
        /// Removes a creator for a key.
        /// </summary>
        public void Unregister(TKey pKey)
        {
            lock (_creators)
            {
                _creators.Remove(pKey);
            }
        }

        /// <summary>
        /// Checks if a creator exists.
        /// </summary>
        public bool HasCreator(TKey pKey)
        {
            lock (_creators)
            {
                return _creators.ContainsKey(pKey);
            }
        }

        /// <summary>
        /// Creates an instance of TClass for the given TKey.
        /// </summary>
        public TClass Create(TKey pKey)
        {
            lock (_creators)
            {
                if (!_creators.ContainsKey(pKey))
                {
                    throw new CreatorException(string.Format("No creator registered for [{0}]", pKey.ToString()));
                }
                return _creators[pKey]();
            }
        }
    }
}
