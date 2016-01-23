using Node.Actions;
using Node.Controls;
using Node.Themes;
using StructureMap;

namespace Node
{
    /// <summary>
    /// Creates a container with all the dependencies needed by the node library.
    /// </summary>
    public static class Dependencies
    {
        /// <summary>
        /// Creates the dependency container.
        /// </summary>
        public static void Bootstrap(iAppTheme pTheme, IContainer pContainer)
        {
            Jobs.Dependencies.Bootstrap(pContainer, 100);

            pContainer.Configure(pExp=>
                                 {
                                     pExp.ForSingletonOf<iActionService>().Use<ActionService>();
                                     pExp.ForSingletonOf<iAppTheme>().Use(pTheme);

                                     pExp.ForSingletonOf<iJobsView>().Use<JobsView>();
                                     pExp.Forward<iJobsView, iActiveJobService>();

                                     pExp.ForSingletonOf<iMainFrmService>().Use<Main>();
                                     pExp.ForSingletonOf<iNodeService>().Use<NodeService>();
                                 });
        }
    }
}