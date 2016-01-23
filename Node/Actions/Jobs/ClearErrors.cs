using System;

namespace Node.Actions.Jobs
{
    /// <summary>
    /// Clears the error flag of all tasks.
    /// </summary>
    internal class ClearErrors : JobAction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ClearErrors()
            : base("Jobs.ClearErrors", true)
        {
        }

        /// <summary>
        /// All actions work on jobs.
        /// </summary>
        protected override void Execute(Guid pJobID)
        {
            JobService.ClearEvents(pJobID, Guid.Empty);
        }

        /// <summary>
        /// Optional if the action doesn't work on tasks.
        /// </summary>
        protected override void Execute(Guid pJobID, Guid pTaskID)
        {
            JobService.ClearEvents(pJobID, pTaskID);
        }
    }
}