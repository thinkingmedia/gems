using System;
using System.Windows.Forms;

namespace Node.Controls
{
    internal class TablessControl : TabControl
    {
        protected override void WndProc(ref Message pMessage)
        {
            // Hide tabs by trapping the TCM_ADJUSTRECT message
            if (pMessage.Msg == 0x1328 && !DesignMode)
            {
                pMessage.Result = (IntPtr)1;
            }
            else
            {
                base.WndProc(ref pMessage);
            }
        }
    }
}