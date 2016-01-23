using System.ComponentModel;

namespace Jobs.Plugins
{
    /// <summary>
    /// Used to hold data associated with a plug-in, and can be
    /// edited by the user interface.
    /// </summary>
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable MemberCanBePrivate.Global
    public class PluginSettings
    {
        /// <summary>
        /// The name of the plug-in.
        /// </summary>
        [Browsable(false)]
        public string Name { get; private set; }

        /// <summary>
        /// The version for these options.
        /// </summary>
        [Browsable(false)]
        public int Version { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pName">The name for this group of options.</param>
        /// <param name="pVersion">The current version</param>
        public PluginSettings(string pName, int pVersion)
        {
            Name = pName;
            Version = pVersion;
        }
    }
}