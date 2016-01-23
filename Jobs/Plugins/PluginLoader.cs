using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Logging;

namespace Jobs.Plugins
{
    /// <summary>
    /// Handles the loading of plug-ins.
    /// </summary>
    internal sealed class PluginLoader : iPluginLoader
    {
        /// <summary>
        /// Logging
        /// </summary>
        private static readonly Logger _logger = Logger.Create(typeof (PluginLoader));

        /// <summary>
        /// A list of plug-ins that are loaded.
        /// </summary>
        private readonly List<iPluginFactory> _plugins;

        /// <summary>
        /// The storage for plug-ins.
        /// </summary>
        private readonly iPluginStorage _storage;

        /// <summary>
        /// Instantiates the iJobFactory object found in a plug-in file.
        /// </summary>
        /// <param name="pType"></param>
        private void LoadFactory(Type pType)
        {
            if (!typeof (iPluginFactory).IsAssignableFrom(pType))
            {
                return;
            }

            _logger.Fine("Found iJobFactory class: {0}", pType);

            iPluginFactory factory = (iPluginFactory)Activator.CreateInstance(pType);
            _plugins.Add(factory);
        }

        /// <summary>
        /// Loads a plugin.dll file as a plug-in resource.
        /// </summary>
        /// <param name="pFile"></param>
        private void LoadPluginFile(string pFile)
        {
            _logger.Fine("Loading plug-in [{0}]", pFile);

            // load the plug-in
            Assembly plugin = Assembly.LoadFile(pFile);

            // find all classes that implement the iJobFactory interface
            Array.ForEach(plugin.GetTypes(), LoadFactory);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PluginLoader(iPluginStorage pStorage)
        {
            _plugins = new List<iPluginFactory>();
            _storage = pStorage;
        }

        /// <summary>
        /// Loads the plug-in DLLs for the engine.
        /// </summary>
        public void Load()
        {
            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.plugin.dll");
            Array.ForEach(files, LoadPluginFile);

            if (_plugins.Count == 0)
            {
                _logger.Error("No plug-in were found.");
            }
        }

        /// <summary>
        /// Creates a PluginStorage object and adds all the settings
        /// from each plug-in.
        /// </summary>
        public void StorePlugins()
        {
            // create the storage for plug-ins, and load it
            foreach (iPluginFactory factory in _plugins)
            {
                _storage.Store(factory.getID(), factory.getSettings());
            }
        }

        /// <summary>
        /// Call once during application start up.
        /// </summary>
        /// <returns>A list of executable jobs, or Null if no plug-ins were found.</returns>
        public List<iJob> Initialize()
        {
            List<iJob> jobs = new List<iJob>();

            foreach (iPluginFactory factory in _plugins)
            {
                try
                {
                    string name = factory.GetType().Module.Assembly.GetName().Name.Replace(".plugin", "");

                    PluginSettings settings = _storage.Find(factory.getID());
                    JobFactory jobFactory = new JobFactory(name, settings);
                    factory.StartUp(settings);
                    factory.Create(jobFactory, settings);
                    jobs.AddRange(jobFactory.Jobs.Values);
                }
                catch (Exception e)
                {
                    _logger.Exception(e);
                    factory.ShutDown();
                }
            }

            return jobs;
        }

        /// <summary>
        /// Unloads all the plug-ins. Should be called before the application
        /// shuts down.
        /// </summary>
        public void Unload()
        {
            foreach (iPluginFactory factory in _plugins)
            {
                try
                {
                    factory.ShutDown();
                }
                catch (Exception e)
                {
                    _logger.Exception(e);
                }
            }
        }
    }
}