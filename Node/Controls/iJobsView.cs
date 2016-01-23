using System.Windows.Forms;

namespace Node.Controls
{
    internal interface iJobsView
    {
        /// <summary>
        /// The UI control to be shown.
        /// </summary>
        Control getControl();

        /// <summary>
        /// Assign the menu to use for a selected job.
        /// </summary>
        void setJobMenu(ContextMenuStrip pMenu);

        /// <summary>
        /// Assign the menu to use for a selected task.
        /// </summary>
        void setTaskMenu(ContextMenuStrip pMenu);
    }
}