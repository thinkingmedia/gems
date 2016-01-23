using System;
using System.Collections.Generic;

namespace Jobs.Plugins
{
    public interface iPluginStorage
    {
        /// <summary>
        /// Retrieves a settings object from storage.
        /// </summary>
        /// <param name="pPluginGUID">The plug-in GUID.</param>
        /// <returns>The settings object</returns>
        PluginSettings Find(Guid pPluginGUID);

        /// <summary>
        /// Loads the storage from disk.
        /// </summary>
        /// <param name="pDirectory">The target directory.</param>
        /// <param name="pFilename">The file (without extension)</param>
        /// <returns>True if successful</returns>
        bool Load(string pDirectory, string pFilename);

        /// <summary>
        /// Saves the storage to disk.
        /// </summary>
        bool Save();

        /// <summary>
        /// Replaces the settings in storage with new settings, or
        /// adds settings to the storage.
        /// </summary>
        /// <param name="pPluginGUID">The plug-in GUID.</param>
        /// <param name="pSettings">The settings object.</param>
        void Store(Guid pPluginGUID, PluginSettings pSettings);

        /// <summary>
        /// Gets a collection of all the settings kept in
        /// storage.
        /// </summary>
        /// <returns>An array of setting objects.</returns>
        IEnumerable<PluginSettings> getSettings();
    }
}