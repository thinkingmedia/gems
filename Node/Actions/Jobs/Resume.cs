using System;

namespace Node.Actions.Jobs
{
    internal class Resume : JobAction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Resume()
            : base("Jobs.Resume")
        {
        }

        /// <summary>
        /// All actions work on jobs.
        /// </summary>
        protected override void Execute(Guid pJobID)
        {
            JobService.Resume(pJobID);
        }
    }
}