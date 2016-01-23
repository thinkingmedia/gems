using System.Collections.Generic;

namespace Node.Actions
{
    /// <summary>
    /// Action handlers implement this interface.
    /// </summary>
    public interface iAction
    {
        /// <summary>
        /// Called to execute the action.
        /// </summary>
        /// <param name="pName"></param>
        void Trigger(string pName);

        /// <summary>
        /// The unique name for this action.
        /// </summary>
        IEnumerable<string> getNames();
    }
}