using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Common.Annotations;

namespace Node.Actions
{
    /// <summary>
    /// Holds all the actions for the application.
    /// </summary>
    internal sealed class ActionService : iActionService
    {
        /// <summary>
        /// Holds all the registered actions.
        /// </summary>
        private readonly Dictionary<string, iAction> _actions;

        /// <summary>
        /// Access an action by it's name.
        /// </summary>
        private iAction Get(string pName)
        {
            if (!_actions.ContainsKey(pName))
            {
                throw new ArgumentException(string.Format("Action [{0}] does not exist.", pName));
            }
            return _actions[pName];
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ActionService()
        {
            _actions = new Dictionary<string, iAction>();
        }

        /// <summary>
        /// Registers an action handler.
        /// </summary>
        public void Register([NotNull] iAction pNewAction)
        {
            if (pNewAction == null)
            {
                throw new NullReferenceException("Argument can not be null.");
            }
            foreach (string name in pNewAction.getNames())
            {
                if (_actions.ContainsKey(name))
                {
                    throw new ArgumentException(string.Format("Action [{0}] already registered.",
                        string.Join(",", pNewAction.getNames())));
                }
                _actions.Add(name, pNewAction);
            }
        }

        /// <summary>
        /// Registers a menu item using it's tag as the action name.
        /// </summary>
        public void Register([NotNull] ToolStripMenuItem pMenuItem)
        {
            if (pMenuItem.Tag == null)
            {
                throw new NullReferenceException(string.Format("Menu [{0}] has no tag.", pMenuItem.Name));
            }
            string name = pMenuItem.Tag.ToString();
            iAction action = Get(name);
            pMenuItem.Click += (pSender, pArgs)=>
                               {
                                   MouseEventArgs mouse = pArgs as MouseEventArgs;
                                   if (mouse != null && mouse.Button != MouseButtons.Left)
                                   {
                                       return;
                                   }
                                   action.Trigger(name);
                               };
        }

        /// <summary>
        /// Triggers an action.
        /// </summary>
        public void Trigger([NotNull] string pName)
        {
            if (pName == null)
            {
                throw new ArgumentNullException("pName");
            }
            if (!_actions.ContainsKey(pName))
            {
                throw new ArgumentException(string.Format("Action [{0}] does not exist.", pName));
            }
            _actions[pName].Trigger(pName);
        }
    }
}