using System.Collections.Generic;
using Common.Events;
using StructureMap;

namespace Node.Actions
{
    /// <summary>
    /// Base class for action handlers.
    /// </summary>
    internal abstract class Action : iAction
    {
        /// <summary>
        /// The main application window.
        /// </summary>
        protected iMainFrmService MainFrm { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        protected Action()
        {
            MainFrm = ObjectFactory.GetInstance<iMainFrmService>();
        }

        /// <summary>
        /// Triggers the action.
        /// </summary>
        /// <param name="pName"></param>
        protected abstract void Execute(string pName);

        /// <summary>
        /// Triggers the action to execute.
        /// </summary>
        /// <param name="pName">The name of the action to trigger.</param>
        public void Trigger(string pName)
        {
            // dispatch the event on the main thread
            FireEvents.Invoke(MainFrm.getMainApplication(), ()=>Execute(pName));
        }

        /// <summary>
        /// The unique name for this action.
        /// </summary>
        public abstract IEnumerable<string> getNames();
    }
}