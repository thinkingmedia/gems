using System;
using DataSources.DataSource;
using Gems.Email.Context;
using Gems.Email.Tasks;
using Jobs;
using Jobs.Context;
using Jobs.Plugins;
using Jobs.States;
using Jobs.Tasks;
using Jobs.Tasks.Events;
using Logging;
using StructureMap;

namespace Gems.Email
{
    public class PluginFactory : iPluginFactory
    {
        /// <summary>
        /// Logging
        /// </summary>
        private static readonly Logger _logger = Logger.Create(typeof(PluginFactory));

        /// <summary>
        /// The ID
        /// </summary>
        private static readonly Guid _guid = new Guid("53063FCF-39A8-4443-8201-284BA50A0E1E");

        /// <summary>
        /// Allows the plug-in to create the required jobs.
        /// </summary>
        public void Create(iJobFactory pJobFactory, PluginSettings pPluginSettings)
        {
            _logger.Fine("Creating jobs.");

            EmailSettings settings = (EmailSettings)pPluginSettings;
            iEventFactory eventFactory = ObjectFactory.GetInstance<iEventFactory>();

            iTaskCollection tasks = ObjectFactory.Container.GetInstance<iTaskCollection>();
            tasks.Add(ObjectFactory.Container.GetInstance<EmailTask>());
            pJobFactory.Create("Status", "status", tasks, new EmailContextFactory(eventFactory), new DailyState(settings.Hour));
        }

        /// <summary>
        /// Allows the plug-in to shut down.
        /// </summary>
        public void ShutDown()
        {
            _logger.Fine("Shutting down.");
        }

        /// <summary>
        /// Allows the plug-in to start up using a custom configuration.
        /// </summary>
        /// <param name="pPluginSettings"></param>
        public void StartUp(PluginSettings pPluginSettings)
        {
            _logger.Fine("Starting up.");

            Dependencies.Bootstrap(ObjectFactory.Container);
        }

        /// <summary>
        /// A unique ID for each plug-in. It should be the same
        /// GUID each time the plug-in is loaded.
        /// </summary>
        /// <returns>GUID</returns>
        public Guid getID()
        {
            return _guid;
        }

        /// <summary>
        /// Provide a reference to the settings object for this plug-in. The
        /// properties of the object will be updated directly by the engine.
        /// </summary>
        /// <returns>Settings object to be saved.</returns>
        public PluginSettings getSettings()
        {
            return new EmailSettings();
        }
    }
}