using System;

namespace Node
{
    public interface iActiveJobService
    {
        /// <summary>
        /// The currently selected Job in the UI.
        /// </summary>
        /// <returns>The ID or Guid.Empty.</returns>
        Guid getActiveJob();

        /// <summary>
        /// The currently selected task for the selected Job.
        /// </summary>
        /// <returns>The ID or Guid.Empty.</returns>
        Guid getActiveTask();

        /// <summary>
        /// Changes the active job.
        /// </summary>
        void setActiveJob(Guid pID);

        /// <summary>
        /// Triggered when the active job changes.
        /// </summary>
        event Action<Guid> JobChanged;

        /// <summary>
        /// Triggered when the active task changed.
        /// </summary>
        event Action<Guid> TaskChanged;
    }
}