using Jobs.Plugins;
using Jobs.Reports;
using Jobs.Tasks;
using Jobs.Tasks.Events;
using StructureMap;

namespace Jobs
{
    /// <summary>
    /// Handles all the object dependencies used by the job library.
    /// </summary>
    public static class Dependencies
    {
        /// <summary>
        /// Creates the injection dependencies.
        /// </summary>
        /// <param name="pContainer">The container to use.</param>
        /// <param name="pEventLimitPerTask">Number of events per task to keep in memory.</param>
        public static void Bootstrap(IContainer pContainer, int pEventLimitPerTask)
        {
            pContainer.Configure(pExp=>
                                 {
                                     pExp.ForSingletonOf<iEngineService>().Use<EngineService>();

                                     // jobs
                                     pExp.ForSingletonOf<iJobService>().Use<JobService>();
                                     pExp.For<iJobReportFactory>().Use<JobReportFactory>();

                                     // plugins
                                     pExp.ForSingletonOf<iPluginStorage>().Use<PluginStorage>();
                                     pExp.For<iPluginLoader>().Use<PluginLoader>();
                                     pExp.For<iTaskCollection>().Use<TaskCollection>();

                                     // events
                                     pExp.ForSingletonOf<iEventRecorderFactory>().Use<EventRecorderFactory>();
                                     pExp.ForSingletonOf<iEventFactory>().Use<EventFactory>();
                                     pExp.For<iEventRecorder>()
                                         .Use<EventRecorder>()
                                         .Ctor<int>("pLimit")
                                         .Is(pEventLimitPerTask);
                                 });
        }
    }
}