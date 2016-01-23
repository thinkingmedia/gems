using System;

namespace Jobs.Plugins
{
    /// <summary>
    /// This interface is used by DLL plug-in to create jobs.
    /// </summary>
    public interface iPluginFactory
    {
        /// <summary>
        /// Allows the plug-in to create the required jobs.
        /// </summary>
        /// <returns>An array of jobs.</returns>
        void Create(iJobFactory pJobFactory, PluginSettings pPluginSettings);

        /// <summary>
        /// Allows the plug-in to shut down.
        /// </summary>
        void ShutDown();

        /// <summary>
        /// Allows the plug-in to start up using a custom configuration.
        /// </summary>
        /// <param name="pPluginSettings"></param>
        void StartUp(PluginSettings pPluginSettings);

        /// <summary>
        /// A unique ID for each plug-in. It should be the same
        /// GUID each time the plug-in is loaded.
        /// </summary>
        /// <returns>GUID</returns>
        Guid getID();

        /// <summary>
        /// Provide a reference to the settings object for this plug-in. The
        /// properties of the object will be updated directly by the engine.
        /// </summary>
        /// <returns>Settings object to be saved.</returns>
        PluginSettings getSettings();
    }
}