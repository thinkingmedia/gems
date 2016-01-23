using System.Collections.Generic;

namespace Node.Actions.Main
{
    internal class ShowMain : Action
    {
        /// <summary>
        /// Triggers the action.
        /// </summary>
        /// <param name="pName"></param>
        protected override void Execute(string pName)
        {
            MainFrm.Show();
        }

        /// <summary>
        /// The unique name for this action.
        /// </summary>
        public override IEnumerable<string> getNames()
        {
            return new[] {"Main.Show"};
        }
    }
}