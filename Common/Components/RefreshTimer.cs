using System;
using System.Windows.Forms;
using GemsLogger;

namespace Common.Components
{
    /// <summary>
    /// Common control used to control refreshing of reports.
    /// </summary>
    public partial class RefreshTimer : UserControl
    {
        private static readonly Logger _logger = Logger.Create(typeof(RefreshTimer));

        /// <summary>
        /// Event that is trigged by the timer.
        /// </summary>
        public event EventHandler onRefresh;

        /// <summary>
        /// Property that defines if the default setting for auto-refresh.
        /// </summary>
        public bool autoRefresh
        {
            get
            {
                return check_auto.Checked;
            }
            set
            {
                check_auto.Checked = value;
            }
        }

        public RefreshTimer()
        {
            InitializeComponent();

            combo_rate.SelectedIndex = 2;
        }

        private void btnClientRefresh_Click(object pSender, EventArgs pEventArgs)
        {
            if (onRefresh != null)
            {
                onRefresh(this, EventArgs.Empty);
            }
        }

        private void refresh_timer_Tick(object pSender, EventArgs pEventArgs)
        {
            if (check_auto.Checked && this.Visible)
            {
                if (onRefresh != null)
                {
                    onRefresh(this, EventArgs.Empty);
                }
            }
        }

        private void combo_rate_SelectedIndexChanged(object pSender, EventArgs pEventArgs)
        {
            switch (combo_rate.SelectedIndex)
            {
                case 0:
                    refresh_timer.Interval = 500;
                    break;
                case 1:
                    refresh_timer.Interval = 1000;
                    break;
                case 2:
                    refresh_timer.Interval = 5000;
                    break;
                case 3:
                    refresh_timer.Interval = 10000;
                    break;
                case 4:
                    refresh_timer.Interval = 30000;
                    break;
                default:
                    _logger.Error("Unknown combo box option.");
                    break;
            }
        }

        private void RefreshTimer_VisibleChanged(object pSender, EventArgs pEventArgs)
        {
            refresh_timer.Enabled = Visible;
        }
    }
}
