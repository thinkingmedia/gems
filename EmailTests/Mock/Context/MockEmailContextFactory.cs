using Gems.Email;
using Gems.Email.Context;
using Jobs.Context;
using Jobs.Plugins;
using Jobs.Tasks.Events;

namespace Gems.EmailTests.Mock.Context
{
    public class MockEmailContextFactory : iJobContextFactory
    {
        /// <summary>
        /// Used to create a job context.
        /// </summary>
        public JobContext Create(PluginSettings pPluginSettings)
        {
            return new EmailContext(pPluginSettings, new EventFactory());
        }

        /// <summary>
        /// Used for testing.
        /// </summary>
        public static EmailContext Create()
        {
            MockEmailContextFactory factory = new MockEmailContextFactory();
            return (EmailContext)factory.Create(new EmailSettings());
        }
    }
}