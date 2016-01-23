using System.Collections.Generic;

namespace Common.Utils
{
    /// <summary>
    /// Handles the storage of persistant objects via a callback
    /// creation method.
    /// </summary>
    /// <typeparam name="TKey">The unique key for each object.</typeparam>
    /// <typeparam name="TValue">The class for each object.</typeparam>
    public class CacheFactory<TKey, TValue>
    {
        /// <summary>
        /// The callback to create the cached object.
        /// </summary>
        /// <returns></returns>
        public delegate TValue Create(TKey pKey);

        /// <summary>
        /// The storage for cached objects
        /// </summary>
        private readonly Dictionary<TKey, TValue> _cache;

        /// <summary>
        /// Constructor
        /// </summary>
        public CacheFactory()
        {
            _cache = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public void Clear()
        {
            lock (_cache)
            {
                _cache.Clear();
            }
        }

        /// <summary>
        /// Will call the callback method to create the object,
        /// and stores it in the cache. On future calls the
        /// cached reference of the object will be returned
        /// instread of creating a new one.
        /// </summary>
        public TValue Init(TKey pKey, Create pCallback)
        {
            lock (_cache)
            {
                if (!_cache.ContainsKey(pKey))
                {
                    _cache.Add(pKey, pCallback(pKey));
                }
                return _cache[pKey];
            }
        }
    }
}