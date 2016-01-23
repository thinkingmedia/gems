using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Common.Events;

namespace Common.Components
{
    /// <summary>
    /// A _user control that displays all the connected clients.
    /// </summary>
    public partial class EventWatcher : UserControl
    {
        /// <summary>
        /// The event logger.
        /// </summary>
        private iEventWatcher _theWatcher;

        /// <summary>
        /// Event handler.
        /// </summary>
        private readonly EventHandler _handler;

        /// <summary>
        /// Watcher property.
        /// </summary>
        public iEventWatcher watcher
        {
            set
            {
                if (_theWatcher != null)
                {
                    _theWatcher.onNewEvent -= _handler;
                }

                _theWatcher = value;

                if (_theWatcher != null)
                {
                    btnTestEMail.Enabled = _theWatcher.canTestEMail();
                    _theWatcher.onNewEvent += _handler;
                }
                else
                {
                    btnTestEMail.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EventWatcher()
        {
            InitializeComponent();

            _handler = new EventHandler(Watcher_OnNewEvent);

            comboLimit.SelectedIndex = 3;
        }

        /// <summary>
        /// Called when a new event has been logged.
        /// </summary>
        void Watcher_OnNewEvent(object pSender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<int>(
                    pX => btnRefresh_Click(this, EventArgs.Empty)), 0);
            }
        }

        /// <summary>
        /// Clears the event Log.
        /// </summary>
        private void btnClear_Click(object pSender, EventArgs e)
        {
            if (_theWatcher == null)
            {
                return;
            }
            if (
                MessageBox.Show(this, @"All event history will be lost. Continue?", @"Warning",
                    MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
            {
                return;
            }
            if (!_theWatcher.clear())
            {
                MessageBox.Show(this, @"Unable to clear event Log.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                btnRefresh_Click(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Refreshes the event list.
        /// </summary>
        private void btnRefresh_Click(object pSender, EventArgs pE)
        {
            if (_theWatcher == null)
            {
                return;
            }
            int amount = 0;
            if (comboLimit.SelectedText != "ALL")
            {
                int.TryParse(comboLimit.SelectedItem.ToString(), out amount);
            }

            IEnumerable<EventLogger.EventRecord> events = _theWatcher.getEvents(amount);

            lvEvents.Items.Clear();
            foreach (EventLogger.EventRecord _event in events)
            {
                DateTime when = _event.When;
                string dateStr = String.Format("{0} {1}", when.ToShortDateString(), when.ToShortTimeString());
                ListViewItem item = lvEvents.Items.Add(dateStr);
                item.SubItems.Add(_event.Type.ToString());
                item.SubItems.Add(_event.Desc);
            }
        }

        /// <summary>
        /// Tests the e-mail delivery.
        /// </summary>
        private void btnTestEMail_Click(object pSender, EventArgs pE)
        {
            if (!_theWatcher.canTestEMail())
            {
                return;
            }
            if (_theWatcher.sendTestEMail())
            {
                MessageBox.Show(this, @"E-Mail has been sent.", @"Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, @"Failed to send test e-mail.", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
