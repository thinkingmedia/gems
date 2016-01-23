using System;
using Node.Themes;

namespace Node
{
    /// <summary>
    /// This is used to start the user interface and main
    /// application.
    /// </summary>
    public static class NodeFactory
    {
        /// <summary>
        /// Has the main node object been created.
        /// </summary>
        private static bool _created;

        /// <summary>
        /// Can only be started once, and should be started by
        /// the Application class.
        /// </summary>
        /// <returns>A new launcher</returns>
        public static iNodeService Create(iAppTheme pTheme)
        {
            if (_created)
            {
                throw new ApplicationException("Node already started.");
            }
            _created = true;
            return new NodeService(pTheme);
        }
    }
}