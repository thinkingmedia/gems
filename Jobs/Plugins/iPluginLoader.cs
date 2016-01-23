using System.Collections.Generic;

namespace Jobs.Plugins
{
    internal interface iPluginLoader
    {
        /// <summary>
        /// Call once during application start up.
        /// </summary>
        /// <returns>A list of executable jobs, or Null if no plug-ins were found.</returns>
        List<iJob> Initialize();

        /// <summary>
        /// Loads the plug-in DLLs for the engine.
        /// </summary>
        void Load();

        /// <summary>
        /// Creates a PluginStorage object and adds all the settings
        /// from each plug-in.
        /// </summary>
        void StorePlugins();

        /// <summary>
        /// Unloads all the plug-ins. Should be called before the application
        /// shuts down.
        /// </summary>
        void Unload();
    }
}