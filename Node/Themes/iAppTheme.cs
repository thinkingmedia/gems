using System.Drawing;

namespace Node.Themes
{
    public interface iAppTheme
    {
        /// <summary>
        /// Shown in the dialogs and taskbar.
        /// </summary>
        Icon Icon { get; }

        /// <summary>
        /// The title
        /// </summary>
        string Title { get; }
    }
}