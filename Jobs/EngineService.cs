using System;
using System.Collections.Generic;
using Common.Events;
using GemsLogger;
using Jobs.Exceptions;
using Jobs.Plugins;
using StructureMap;

namespace Jobs
{
    /// <summary>
    /// The main class used to control the Engine.
    /// </summary>
    public sealed class EngineService : iEngineService
    {
        /// <summary>
        /// Logging
        /// </summary>
        private static readonly Logger _logger = Logger.Create(typeof (EngineService));

        /// <summary>
        /// The job manager.
        /// </summary>
        private readonly iJobService _jobService;

        /// <summary>
        /// Has the Engine been loaded
        /// </summary>
        private bool _loaded;

        /// <summary>
        /// Handles the loading of plug-ins.
        /// </summary>
        private iPluginLoader _pluginLoader;

        /// <summary>
        /// True if the Engine has been started.
        /// </summary>
        private bool _running;

        bool iEngineService.isLoaded()
        {
            return _loaded;
        }

        bool iEngineService.isRunning()
        {
            return _running;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EngineService()
        {
            _jobService = ObjectFactory.GetInstance<iJobService>();

            _running = false;
            _loaded = false;
        }

        /// <summary>
        /// Before the Engine can be started. It has to be loaded.
        /// This will load all the plug-ins, and initialize their
        /// stored settings.
        /// </summary>
        public void Load()
        {
            if (_loaded)
            {
                throw new EngineException("Engine has already been loaded.");
            }
            _loaded = true;

            // load the plug-ins
            _pluginLoader = ObjectFactory.GetInstance<iPluginLoader>();
            _pluginLoader.Load();

            // create the storage and load plug-in settings
            _pluginLoader.StorePlugins();
        }

        /// <summary>
        /// Starts the Engine. This should be called once at
        /// application start up and called from the main UI thread.
        /// </summary>
        public void Start()
        {
            if (!_loaded)
            {
                throw new EngineException("Engine has not been loaded.");
            }
            if (_running)
            {
                throw new EngineException("Engine is already started.");
            }

            _running = true;

            FireEvents.Empty(this, onStart);

            List<iJob> jobs = _pluginLoader.Initialize();
            if (jobs.Count == 0)
            {
                throw new JobException("There are no jobs to execute.");
            }

            jobs.ForEach(_jobService.Start);
        }

        /// <summary>
        /// Stops the Engine. This should be called before
        /// the application exits.
        /// </summary>
        public void Stop()
        {
            FireEvents.Empty(this, onStop);

            _jobService.StopAll();
            _pluginLoader.Unload();
        }

        /// <summary>
        /// Called when the engine is started.
        /// </summary>
        public event EventHandler onStart;

        /// <summary>
        /// Called when the engine is stopped.
        /// </summary>
        public event EventHandler onStop;
    }
}