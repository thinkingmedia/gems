using System;

namespace Node.Actions.Jobs
{
    internal class Suspend : JobAction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Suspend()
            : base("Jobs.Suspend")
        {
        }

        /// <summary>
        /// All actions work on jobs.
        /// </summary>
        protected override void Execute(Guid pJobID)
        {
            JobService.Suspend(pJobID);
        }
    }
}