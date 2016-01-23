using System;
using System.Collections.Generic;
using Jobs.Context;
using Jobs.Exceptions;
using Jobs.Plugins;
using Jobs.States;
using Jobs.Tasks;
using Jobs.Tasks.Events;
using Logging;
using StructureMap;

namespace Jobs
{
    /// <summary>
    /// A factory class for creating jobs.
    /// </summary>
    internal sealed class JobFactory : iJobFactory
    {
        /// <summary>
        /// A list of created jobs.
        /// </summary>
        public readonly Dictionary<Guid, iJob> Jobs;

        /// <summary>
        /// Logging
        /// </summary>
        private readonly Logger _logger = Logger.Create(typeof (JobFactory));

        /// <summary>
        /// The settings for the plug-in.
        /// </summary>
        private readonly PluginSettings _settings;

        /// <summary>
        /// Creates a new job object.
        /// </summary>
        private Guid CreateJob(
            Guid pParentID,
            string pName,
            string pCode,
            iTaskCollection pTaskCollection,
            iJobContextFactory pJobContextFactory,
            iJobState pJobState,
            int pMaxErrors)
        {
            if (string.IsNullOrWhiteSpace(pName))
            {
                throw new NullReferenceException("Invalid name for a job.");
            }
            if (string.IsNullOrWhiteSpace(pCode))
            {
                throw new NullReferenceException("Invalid code for a job.");
            }
            if (pTaskCollection == null)
            {
                throw new NullReferenceException("pTaskFactory can not be null.");
            }
            if (pJobContextFactory == null)
            {
                throw new NullReferenceException("pContextFactory can not be null.");
            }
            if (pJobState == null)
            {
                throw new NullReferenceException("pJobState can not be null.");
            }

            Job job = new Job(
                pParentID,
                _settings,
                Name,
                pName,
                pCode,
                pTaskCollection,
                pJobContextFactory,
                ObjectFactory.Container.GetInstance<iEventFactory>(),
                pJobState,
                pMaxErrors);

            Jobs.Add(job.ID, job);
            return job.ID;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public JobFactory(string pName, PluginSettings pSettings)
        {
            Name = pName;

            _settings = pSettings;

            Jobs = new Dictionary<Guid, iJob>();
        }

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
        public Guid Create(Guid pParent, string pName, string pCode, iTaskCollection pTaskCollection,
                           iJobContextFactory pJobContextFactory,
                           iJobState pJobState, int pMaxErrors = 0)
        {
            if (!Jobs.ContainsKey(pParent))
            {
                throw new JobException("Parent job does not exist.");
            }

            Guid child = CreateJob(pParent, pName, pCode, pTaskCollection, pJobContextFactory, pJobState, pMaxErrors);

            _logger.Fine("Job created:{0} Depends On:{1}", Jobs[child], Jobs[pParent]);

            return child;
        }

        /// <summary>
        /// The settings for the plug-in.
        /// </summary>
        /// <returns>PluginSettings object</returns>
        public PluginSettings getSettings()
        {
            return _settings;
        }

        /// <summary>
        /// The name of the plug-in this factory was created for.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Creates a new runnable job.
        /// </summary>
        /// <param name="pName">Name of the job.</param>
        /// <param name="pCode"></param>
        /// <param name="pTaskCollection">Factory to create tasks for the job.</param>
        /// <param name="pJobContextFactory">Factory to create a context for the job.</param>
        /// <param name="pJobState">Object to control the state of the job.</param>
        /// <param name="pMaxErrors">The job's max errors.</param>
        /// <returns>A new job object.</returns>
        public Guid Create(string pName, string pCode, iTaskCollection pTaskCollection, iJobContextFactory pJobContextFactory,
                           iJobState pJobState, int pMaxErrors = 0)
        {
            Guid id = CreateJob(Guid.Empty, pName, pCode, pTaskCollection, pJobContextFactory, pJobState, pMaxErrors);

            _logger.Fine("Job created:{0}", Jobs[id]);

            return id;
        }
    }
}