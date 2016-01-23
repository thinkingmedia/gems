using Jobs.Plugins;

namespace Jobs.Context
{
    /// <summary>
    /// Used to create a JobContext for a worker thread.
    /// </summary>
    public interface iJobContextFactory
    {
        /// <summary>
        /// Used to create a job context.
        /// </summary>
        /// <param name="pPluginSettings"></param>
        JobContext Create(PluginSettings pPluginSettings);
    }
}