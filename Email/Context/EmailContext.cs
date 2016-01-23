using DataSources.DataSource;
using Jobs.Context;
using Jobs.Plugins;
using Jobs.Tasks.Events;

namespace Gems.Email.Context
{
    public class EmailContext : JobContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EmailContext(PluginSettings pPluginSettings, iEventFactory pEventFactory)
            : base(pPluginSettings, null, pEventFactory)
        {
        }
    }
}