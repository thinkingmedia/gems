using System.Collections.Generic;

namespace Node.Actions.Main
{
    internal class Shutdown : Action
    {
        /// <summary>
        /// Triggers the action.
        /// </summary>
        /// <param name="pName"></param>
        protected override void Execute(string pName)
        {
            MainFrm.Shutdown();
        }

        /// <summary>
        /// The unique name for this action.
        /// </summary>
        public override IEnumerable<string> getNames()
        {
            return new[] {"Main.Shutdown"};
        }
    }
}