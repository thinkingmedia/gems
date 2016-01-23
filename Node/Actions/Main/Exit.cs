using System.Collections.Generic;
using System.Windows.Forms;
using Node.Properties;

namespace Node.Actions.Main
{
    /// <summary>
    /// Exists the application
    /// </summary>
    internal class Exit : Action
    {
        /// <summary>
        /// Display a prompt to exit the application.
        /// </summary>
        /// <param name="pName"></param>
        protected override void Execute(string pName)
        {
            DialogResult result = MessageBox.Show((IWin32Window)MainFrm, Resources.Shutdown_Message,
                Resources.Shutdown_Title,
                MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                MainFrm.Exit();
            }
        }

        /// <summary>
        /// The unique name for this action.
        /// </summary>
        public override IEnumerable<string> getNames()
        {
            return new[] {"Main.Exit"};
        }
    }
}