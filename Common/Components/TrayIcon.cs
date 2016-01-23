using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Common.Components
{
    /// <summary>
    /// Displays the current status of the Monitor in the system tray.
    /// </summary>
    public partial class TrayIcon : Component
    {
        private Form _parentForm;
        private bool _flashState = false;
        private Icon _flashingIcon;

        /// <summary>
        /// Sets the parent form that can have it's icon updated as well.
        /// </summary>
        public Form parent
        {
            set
            {
                _parentForm = value;
                setIcon(notifyIcon1.Icon);
            }
        }

        /// <summary>
        /// Sets the text shown for mouse over.
        /// </summary>
        public string text
        {
            get
            {
                return notifyIcon1.Text;
            }
            set
            {
                notifyIcon1.Text = value;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TrayIcon()
        {
            InitializeComponent();

            normal();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TrayIcon(IContainer pContainer)
        {
            pContainer.Add(this);

            InitializeComponent();

            normal();
        }

        /// <summary>
        /// Sets the icon on both the try and parent form.
        /// </summary>
        private void setIcon(Icon pIcon)
        {
            notifyIcon1.Icon = pIcon;
            if (_parentForm != null)
            {
                _parentForm.Icon = pIcon;
            }
        }

        /// <summary>
        /// Sets the normal status icon.
        /// </summary>
        private void normal()
        {
            setIcon(Properties.Resources.server);
            timerFlashIcon.Enabled = false;
        }

        /// <summary>
        /// Sets the connected status icon.
        /// </summary>
        public void connected()
        {
            setIcon(Properties.Resources.enable_server);
            timerFlashIcon.Enabled = false;
        }

        /// <summary>
        /// Sets a warning status icon.
        /// </summary>
        /// <param name="pMsg"></param>
        public void warning(string pMsg)
        {
            _flashingIcon = Properties.Resources.desable_server;
            timerFlashIcon.Enabled = true;
            notifyIcon1.ShowBalloonTip(5000, "Warning", pMsg, ToolTipIcon.Warning);
        }

        /// <summary>
        /// Sets an alert status icon.
        /// </summary>
        public void alert(string pMsg)
        {
            _flashingIcon = Properties.Resources.firewall_server;
            timerFlashIcon.Enabled = true;
            notifyIcon1.ShowBalloonTip(5000, "Alert", pMsg, ToolTipIcon.Error);
        }

        /// <summary>
        /// Displays an information popup.
        /// </summary>
        /// <param name="pMsg"></param>
        public void info(string pMsg)
        {
            notifyIcon1.ShowBalloonTip(5000, "Info", pMsg, ToolTipIcon.Info);
        }

        public void tip(string pMessage)
        {
            notifyIcon1.BalloonTipText = pMessage;
            notifyIcon1.ShowBalloonTip(5000);
        }

        /// <summary>
        /// Flashes the icon.
        /// </summary>
        private void timerFlashIcon_Tick(object pSender, EventArgs pEventArgs)
        {
            setIcon(_flashState ? _flashingIcon : Properties.Resources.server);
            _flashState = !_flashState;
        }

        private void notifyIcon1_DoubleClick(object pSender, EventArgs pEventArgs)
        {
            if (_parentForm != null)
            {
                _parentForm.Show();
                _parentForm.WindowState = FormWindowState.Normal;
            }
        }
    }
}
