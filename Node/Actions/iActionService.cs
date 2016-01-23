using System.Windows.Forms;

namespace Node.Actions
{
    public interface iActionService
    {
        void Register(iAction pNewAction);
        void Register(ToolStripMenuItem pMenuItem);
        void Trigger(string pName);
    }
}