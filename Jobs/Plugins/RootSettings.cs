using System;
using System.Collections.Generic;
using System.Linq;
using GemsCollections.Dictionary;

namespace Jobs.Plugins
{
    /// <summary>
    /// Holds all the settings for the storage
    /// </summary>
    public sealed class RootSettings
    {
        /// <summary>
        /// All the settings.
        /// </summary>
        private readonly Dictionary<Guid, PluginSettings> _settings;

        /// <summary>
        /// Constructor
        /// </summary>
        public RootSettings()
        {
            _settings = new Dictionary<Guid, PluginSettings>();
        }

        /// <summary>
        /// Retrieves the collection as a read-only dictionary.
        /// </summary>
        /// <returns>The dictionary</returns>
        public IEnumerable<KeyValuePair<Guid, PluginSettings>> ToDictionary()
        {
            return new ReadOnlyDictionary<Guid, PluginSettings>(_settings);
        }

        /// <summary>
        /// Retrieves a collection of all the settings in storage.
        /// </summary>
        /// <returns>The collection of settings</returns>
        public IEnumerable<PluginSettings> ToList()
        {
            return _settings.Values.ToList();
        }

        /// <summary>
        /// Retrieves the settings for a plug-in.
        /// </summary>
        /// <param name="pPluginGUID">The plug-in GUID</param>
        /// <returns>The settings or Null.</returns>
        public PluginSettings get(Guid pPluginGUID)
        {
            return _settings[pPluginGUID];
        }

        /// <summary>
        /// Stores settings for a plug-in.
        /// </summary>
        /// <param name="pPluginGUID">The plug-in GUID</param>
        /// <param name="pSettings">The settings.</param>
        public void set(Guid pPluginGUID, PluginSettings pSettings)
        {
            if (pSettings == null)
            {
                throw new ArgumentNullException("pSettings", @"Settings can not be null.");
            }
            _settings[pPluginGUID] = pSettings;
        }
    }
}