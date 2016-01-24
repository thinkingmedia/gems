using System.Collections.Generic;
using System.Windows.Forms;
using GemsLogger;
using Jobs.Plugins;
using Node.Properties;

namespace Node
{
    /// <summary>
    /// A properties editor for PlugInOptions objects.
    /// </summary>
    internal partial class Options : Form
    {
        /// <summary>
        /// Logging
        /// </summary>
        private static readonly Logger _logger = Logger.Create(typeof (Options));

        private readonly iPluginStorage _storage;

        /// <summary>
        /// The name of the property being changed.
        /// </summary>
        private string _oldName;

        /// <summary>
        /// The old value for a property.
        /// </summary>
        private string _oldValue;

        /// <summary>
        /// Recreates the tree list of options.
        /// </summary>
        private void UpdateStorage()
        {
            IEnumerable<PluginSettings> settings = _storage.getSettings();

            tree.BeginUpdate();
            tree.Nodes.Clear();

            foreach (PluginSettings option in settings)
            {
                TreeNode node = new TreeNode(option.Name) {Tag = option};
                tree.Nodes.Add(node);
            }

            if (tree.Nodes.Count > 0)
            {
                tree.SelectedNode = tree.Nodes[0];
            }

            tree.EndUpdate();
        }

        /// <summary>
        /// Remembers the old value of a property
        /// </summary>
        /// <param name="pSender"></param>
        /// <param name="pEventArgs"></param>
        private void editor_SelectedGridItemChanged(object pSender, SelectedGridItemChangedEventArgs pEventArgs)
        {
            _oldName = pEventArgs.NewSelection.Label ?? "";
            object val = pEventArgs.NewSelection.Value ?? "";
            _oldValue = (_oldName.ToLower() == "password") ? "*****" : val.ToString();
        }

        /// <summary>
        /// Record the changes made to properties.
        /// </summary>
        /// <param name="pSender"></param>
        /// <param name="pEventArgs"></param>
        private void onChanged(object pSender, PropertyValueChangedEventArgs pEventArgs)
        {
            string name = pEventArgs.ChangedItem.Label ?? "";
            object val = pEventArgs.ChangedItem.Value ?? "";
            val = (name.ToLower() == "password") ? "*****" : val;
            _logger.Fine("Changed {0}:{1} to {2}:{3}", _oldName, _oldValue, name, val);
        }

        /// <summary>
        /// When the dialog is closing.
        /// </summary>
        /// <param name="pSender"></param>
        /// <param name="pEventArgs"></param>
        private void onClosing(object pSender, FormClosingEventArgs pEventArgs)
        {
            if (_storage.Save())
            {
                return;
            }

            if (DialogResult.Yes == MessageBox.Show(
                Resources.OptionsFailedSavingMessage,
                Resources.OptionsFailedSaving,
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button3))
            {
                return;
            }

            pEventArgs.Cancel = true;
        }

        /// <summary>
        /// When a tree node is clicked.
        /// </summary>
        /// <param name="pSender"></param>
        /// <param name="pEventArgs"></param>
        private void onNodeClick(object pSender, TreeNodeMouseClickEventArgs pEventArgs)
        {
            editor.SelectedObject = pEventArgs.Node.Tag;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Options(iPluginStorage pStorage)
        {
            _storage = pStorage;

            InitializeComponent();
            UpdateStorage();
            editor.SelectedObject = (tree.SelectedNode == null) ? null : tree.SelectedNode.Tag;
        }
    }
}