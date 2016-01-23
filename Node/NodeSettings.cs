using System.ComponentModel;
using System.Windows.Forms;
using Common.Events;
using Common.Utils;
using Jobs.Plugins;

namespace Node
{
    /// <summary>
    /// These are the options for the node UI.
    /// </summary>
    // ReSharper disable InconsistentNaming
    // ReSharper disable MemberCanBePrivate.Global
    internal class NodeSettings : PluginSettings, INotifyPropertyChanged
    {
        private bool _hideOnMinimize;
        private bool _minimizeOnClose;
        private bool _stayOnTop;

        [Category("Window")]
        [Description("Hide to the tray icon when minimized.")]
        [DefaultValue(true)]
        public bool HideOnMinimize
        {
            get { return _hideOnMinimize; }
            set
            {
                _hideOnMinimize = value;
                FireEvents.PropertyChanged(this, PropertyChanged, "HideOnMinimize");
            }
        }

        [Category("Jobs")]
        [Description("The last running job that was being viewed.")]
        [DisplayName(@"Job Code")]
        public string JobCode { get; set; }

        [Category("Window")]
        [Description("Minimize on close or hide to tray icon.")]
        [DefaultValue(true)]
        public bool MinimizeOnClose
        {
            get { return _minimizeOnClose; }
            set
            {
                _minimizeOnClose = value;
                FireEvents.PropertyChanged(this, PropertyChanged, "MinimizeOnClose");
            }
        }

        [Category("Window")]
        [Description("Stay on top of other windows.")]
        [DefaultValue(true)]
        public bool StayOnTop
        {
            get { return _stayOnTop; }
            set
            {
                _stayOnTop = value;
                FireEvents.PropertyChanged(this, PropertyChanged, "StayOnTop");
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public NodeSettings()
            : base("Node", 1)
        {
            StayOnTop = true;
            MinimizeOnClose = true;
            HideOnMinimize = true;
            JobCode = "";
        }

        /// <summary>
        /// Change notifications
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Binds a menu's checked state to a property as a boolean type.
        /// </summary>
        /// <param name="pItem">The menu item.</param>
        /// <param name="pPropertyNamne">The property to bind.</param>
        public void bindMenu(ToolStripMenuItem pItem, string pPropertyNamne)
        {
            pItem.Checked = Reflection.getProperty<bool>(this, pPropertyNamne);
            pItem.Click += (pSender, pArgs)=>
                           {
                               bool value = !pItem.Checked;
                               Reflection.setProperty(this, pPropertyNamne, value);
                           };
            PropertyChanged +=
                (pSender, pArgs)=> { pItem.Checked = Reflection.getProperty<bool>(this, pPropertyNamne); };
        }
    }
}