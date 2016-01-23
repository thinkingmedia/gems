using System;
using System.Collections.Generic;
using System.Linq;
using Jobs.Exceptions;

namespace Jobs.Plugins
{
    /// <summary>
    /// Handles the serializing of plug-in settings to disk
    /// and back.
    /// </summary>
    internal class PluginStorage : iPluginStorage
    {
        /// <summary>
        /// All settings are stored as children of the room.
        /// </summary>
        private readonly Dictionary<Guid, PluginSettings> _settings;

        /// <summary>
        /// The path to the storage file. Set when the storage
        /// is loaded.
        /// </summary>
        private string _path;

        /// <summary>
        /// Constructor
        /// </summary>
        public PluginStorage()
        {
            _settings = new Dictionary<Guid, PluginSettings>();
        }

        /// <summary>
        /// Replaces the settings in storage with new settings, or
        /// adds settings to the storage.
        /// </summary>
        /// <param name="pPluginGUID">The plug-in GUID.</param>
        /// <param name="pSettings">The settings object.</param>
        public void Store(Guid pPluginGUID, PluginSettings pSettings)
        {
            lock (_settings)
            {
                if (_settings.ContainsKey(pPluginGUID))
                {
                    throw new StorageException("Settings for plug-in already stored. [{0}]", pPluginGUID);
                }
                _settings.Add(pPluginGUID, pSettings);
            }
        }

        /// <summary>
        /// Retrieves a settings object from storage.
        /// </summary>
        /// <param name="pPluginGUID">The plug-in GUID.</param>
        /// <returns>The settings object</returns>
        public PluginSettings Find(Guid pPluginGUID)
        {
            lock (_settings)
            {
                if (_settings.ContainsKey(pPluginGUID))
                {
                    return _settings[pPluginGUID];
                }
            }
            throw new StorageException("Settings for plug-in not found [{0}]", pPluginGUID);
        }

        /// <summary>
        /// Gets a collection of all the settings kept in
        /// storage.
        /// </summary>
        /// <returns>An array of setting objects.</returns>
        public IEnumerable<PluginSettings> getSettings()
        {
            lock (_settings)
            {
                return _settings.Values.ToList();
            }
        }

        /// <summary>
        /// Saves the storage to disk.
        /// </summary>
        public bool Save()
        {
            lock (_settings)
            {
                if (_settings.Count == 0)
                {
                    throw new StorageException("Nothing to save. Storage is empty.");
                }
                return SerializeSettings.Save(_path, _settings);
            }
        }

        /// <summary>
        /// Loads the storage from disk.
        /// </summary>
        /// <param name="pDirectory">The target directory.</param>
        /// <param name="pFilename">The file (without extension)</param>
        /// <returns>True if successful</returns>
        public bool Load(string pDirectory, string pFilename)
        {
            lock (_settings)
            {
                _path = string.Format("{0}\\{1}.ini", pDirectory, pFilename);
                return SerializeSettings.Load(_path, _settings);
            }
        }
    }
}