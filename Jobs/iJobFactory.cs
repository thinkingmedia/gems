using System;
using Jobs.Context;
using Jobs.Plugins;
using Jobs.States;
using Jobs.Tasks;

namespace Jobs
{
    /// <summary>
    /// Used to create jobs in the engine.
    /// </summary>
    public interface iJobFactory
    {
        /// <summary>
        /// The name of the plug-in this factory was created for.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Creates a new runnable job.
        /// </summary>
        /// <param name="pName">Name of the job.</param>
        /// <param name="pCode">Unique code for this job (used by the logger)</param>
        /// <param name="pTaskCollection">Factory to create tasks for the job.</param>
        /// <param name="pJobContextFactory">Factory to create a context for the job.</param>
        /// <param name="pJobState">Object to control the state of the job.</param>
        /// <param name="pMaxErrors">The job's max errors.</param>
        /// <returns>A new job object.</returns>
        Guid Create(string pName, string pCode, iTaskCollection pTaskCollection, iJobContextFactory pJobContextFactory,
                    iJobState pJobState, int pMaxErrors = 0);

        /// <summary>
        /// Creates a new runnable job.
        /// </summary>
        /// <param name="pParent">Parent job must finish first before this job starts.</param>
        /// <param name="pName">Name of the job.</param>
        /// <param name="pCode">Unique code for this job (used by the logger)</param>
        /// <param name="pTaskCollection">Factory to create tasks for the job.</param>
        /// <param name="pJobContextFactory">Factory to create a context for the job.</param>
        /// <param name="pJobState">Object to control the state of the job.</param>
        /// <param name="pMaxErrors">The job's max errors.</param>
        /// <returns>A new job object.</returns>
        Guid Create(Guid pParent, string pName, string pCode, iTaskCollection pTaskCollection, iJobContextFactory pJobContextFactory,
                    iJobState pJobState, int pMaxErrors = 0);

        /// <summary>
        /// The settings for the plug-in.
        /// </summary>
        /// <returns>PluginSettings object</returns>
        PluginSettings getSettings();
    }
}