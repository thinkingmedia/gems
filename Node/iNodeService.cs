using System.Windows.Forms;
using Node.Themes;

namespace Node
{
    /// <summary>
    /// Gives access to the inner launcher for the Node's
    /// user interface.
    /// </summary>
    public interface iNodeService
    {
        /// <summary>
        /// The main application window.
        /// </summary>
        /// <returns></returns>
        Form getMainWindow();

        /// <summary>
        /// The application theme
        /// </summary>
        /// <returns></returns>
        iAppTheme getTheme();
    }
}