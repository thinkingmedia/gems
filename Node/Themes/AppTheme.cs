using System;
using System.Drawing;
using System.Reflection;
using Node.Properties;

namespace Node.Themes
{
    /// <summary>
    /// Contains the details to theme the node with
    /// branding graphics and text.
    /// </summary>
    internal sealed class AppTheme : iAppTheme
    {
        /// <summary>
        /// Shown in the dialogs and taskbar.
        /// </summary>
        public Icon Icon { get; private set; }

        /// <summary>
        /// The title
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        private AppTheme(string pTitle)
        {
            if (pTitle == null)
            {
                throw new ArgumentNullException("pTitle");
            }

            Title = string.Format(@"{0} [v.{1}]", pTitle, Assembly.GetExecutingAssembly().GetName().Version);
            Icon = Resources.scheduled_tasks;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AppTheme(string pTitle, Icon pIcon)
            : this(pTitle)
        {
            if (pIcon == null)
            {
                throw new ArgumentNullException("pIcon");
            }

            Icon = pIcon;
        }
    }
}