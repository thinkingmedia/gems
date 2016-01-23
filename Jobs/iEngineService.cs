using System;

namespace Jobs
{
    public interface iEngineService
    {
        /// <summary>
        /// The engine needs to load plugins. This should be called before starting.
        /// </summary>
        void Load();

        /// <summary>
        /// Starts the engine service. Should only be called once during application start up.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the engine service. Call before exiting the application.
        /// </summary>
        void Stop();

        /// <summary>
        /// Tells if the engine has been started.
        /// </summary>
        bool isLoaded();

        /// <summary>
        /// Tells if the engine has been started.
        /// </summary>
        bool isRunning();

        /// <summary>
        /// Called when the engine is started.
        /// </summary>
        event EventHandler onStart;

        /// <summary>
        /// Called when the engine is stopped.
        /// </summary>
        event EventHandler onStop;
    }
}