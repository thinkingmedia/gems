using DataSources.DataSource;
using Jobs.Context;
using Jobs.Plugins;
using Jobs.Tasks.Events;
using StructureMap;

namespace Gems.Email.Context
{
    public class EmailContextFactory : iJobContextFactory
    {
        /// <summary>
        /// The event factory to use.
        /// </summary>
        private readonly iEventFactory _eventFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailContextFactory(iEventFactory pEventFactory)
        {
            _eventFactory = pEventFactory;
        }

        /// <summary>
        /// Used to create a job context.
        /// </summary>
        /// <param name="pPluginSettings"></param>
        public JobContext Create(PluginSettings pPluginSettings)
        {
            return new EmailContext(pPluginSettings, _eventFactory);
        }
    }
}