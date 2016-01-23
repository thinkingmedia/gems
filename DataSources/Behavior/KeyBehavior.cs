using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DataSources.Query;

namespace DataSources.Behavior
{
    /// <summary>
    /// Updates a key field from the title field by
    /// converting the title to underscores.
    /// </summary>
    public class KeyBehavior : Behavior
    {
        /// <summary>
        /// The name of the key field.
        /// </summary>
        public readonly string Key;

        /// <summary>
        /// Flag controls usage of the cache.
        /// </summary>
        private readonly bool _cache = true;

        /// <summary>
        /// Cache of IDs by their key.
        /// </summary>
        private readonly Dictionary<string, uint> _cacheId;

        /// <summary>
        /// Cache of Records by their key.
        /// </summary>
        private readonly Dictionary<string, Record> _cacheRecord;

        /// <summary>
        /// The underscore character for spaces.
        /// </summary>
        private readonly string _seperator;

        /// <summary>
        /// The name of the title field.
        /// </summary>
        private readonly string _title;

        /// <summary>
        /// Clears the memory cache.
        /// </summary>
        private void Clear()
        {
            _cacheId.Clear();
            _cacheRecord.Clear();
        }

        /// <summary>
        /// Writes a value to the cache.
        /// </summary>
        private T UpdateCache<T>(IDictionary<string, T> pCache, string pKey, T pValue)
        {
            if (!_cache)
            {
                return pValue;
            }
            if (pCache.ContainsKey(pKey))
            {
                pCache.Remove(pKey);
            }
            pCache.Add(pKey, pValue);
            return pValue;
        }

        /// <summary>
        /// Assigns the key value to the model's data.
        /// </summary>
        [Obsolete]
        private bool UpdateKey()
        {
            if (!Model.Data.Has(_title))
            {
                return true;
            }
            string value = Format(Model.Data.getString(_title));
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            Model.Data[Key] = value;
            return true;
        }

        /// <summary>
        /// Assigns the key value to the model's data.
        /// </summary>
        private bool UpdateKey(iQueryBuilder pQuery)
        {
            // if the TITLE column IS NOT being assigned a value, then abort.
            string title = pQuery.getValue(_title) as string;
            if(title == null)
            {
                return true;
            }

            // if the KEY column IS being assigned a value, then abort
            string key = pQuery.getValue(Key) as string;
            if (key != null)
            {
                return true;
            }

            key = Format(title);
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            pQuery.Set(Key, key);
            return true;
        }

        /// <summary>
        /// Checks if a cache contains a key.
        /// </summary>
        private bool isCached<T>(IDictionary<string, T> pCache, string pKey)
        {
            return _cache && pCache.ContainsKey(pKey);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public KeyBehavior(string pKey = "key", string pTitle = "title", string pSeperator = "-", bool pCaching = true)
        {
            Key = pKey;
            _title = pTitle;
            _seperator = pSeperator;

            _cacheId = new Dictionary<string, uint>();
            _cacheRecord = new Dictionary<string, Record>();
            _cache = pCaching;
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public override void AfterInsert()
        {
            Clear();
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public override void AfterDelete()
        {
            Clear();
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public override void AfterUpdate()
        {
            Clear();
        }

        /// <summary>
        /// Update the key column when creating data.
        /// </summary>
        public override bool BeforeInsert(QueryBuilder pQuery)
        {
            return UpdateKey(pQuery);
        }

        /// <summary>
        /// Update the key column when saving data.
        /// </summary>
        public override bool BeforeUpdate(QueryBuilder pQuery)
        {
            return UpdateKey(pQuery);
        }

        /// <summary>
        /// Converts a title into a key.
        /// </summary>
        public static string ToKey(string pTitle, string pSeperator = "-")
        {
            string str = pTitle.Trim().ToLower();
            str = Regex.Replace(str, "[^A-Za-z0-9]", " ");
            str = Regex.Replace(str, "[ ]{2,}", " ");
            str = str.Trim();
            str = str.Replace(" ", pSeperator);
            return str;
        }

        /// <summary>
        /// Converts the title into a key.
        /// </summary>
        public string Format(string pTitle)
        {
            return ToKey(pTitle, _seperator);
        }

        /// <summary>
        /// Finds the ID of a record using the key.
        /// </summary>
        /// <param name="pKey">The key to find.</param>
        /// <exception cref="Exceptions.KeyNotFoundException">If the key is not found.</exception>
        /// <returns>The ID</returns>
        public UInt32 getID(string pKey)
        {
            if (isCached(_cacheId, pKey))
            {
                return _cacheId[pKey];
            }

            UInt32 recordID = Model.Select(Model.Settings.PrimaryKey)
                .Where()
                .isKey(pKey)
                .End()
                .Scalar<UInt32>();

            return UpdateCache(_cacheId, pKey, recordID);
        }

        /// <summary>
        /// Checks if a record exists with this Key.
        /// </summary>
        /// <param name="pKey">The key to check.</param>
        /// <returns>True if it exists.</returns>
        public bool HasKey(string pKey)
        {
            if (isCached(_cacheId, pKey))
            {
                return true;
            }

            return Model.Count()
                .Where()
                .isKey(pKey)
                .End()
                .Scalar<UInt32>() == 1;
        }
    }
}