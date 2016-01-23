using System.Collections.Generic;
using Jobs.Plugins;
using StructureMap;

namespace Node.Actions.Main
{
    internal class EditOptions : Action
    {
        /// <summary>
        /// Triggers the action.
        /// </summary>
        /// <param name="pName"></param>
        protected override void Execute(string pName)
        {
            Options options = new Options(ObjectFactory.Container.GetInstance<iPluginStorage>());
            options.ShowDialog(MainFrm.getMainApplication());
        }

        /// <summary>
        /// The unique name for this action.
        /// </summary>
        public override IEnumerable<string> getNames()
        {
            return new[] {"Main.Options"};
        }
    }
}