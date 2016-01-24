using System;
using Common.Annotations;
using DataSources.DataSource;
using GemsLogger;
using Jobs.Plugins;
using Jobs.Tasks.Events;

namespace Jobs.Context
{
    /// <summary>
    /// Holds information about the currently running job.
    /// </summary>
    public abstract class JobContext : iEventContext, IDisposable
    {
        /// <summary>
        /// Logging
        /// </summary>
        private static readonly Logger _logger = Logger.Create(typeof (JobContext));

        /// <summary>
        /// The database connection.
        /// </summary>
        public readonly iDataSource Source;

        /// <summary>
        /// Used to create events.
        /// </summary>
        private readonly iEventFactory _eventFactory;

        /// <summary>
        /// Holds the report object for the currently
        /// executing task.
        /// </summary>
        private iEventRecorder _eventRecorder;

        /// <summary>
        /// The settings created by the JobFactory
        /// </summary>
        public PluginSettings PluginSettings { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        protected JobContext([NotNull] PluginSettings pPluginSettings,
                             [CanBeNull] iDataSource pDataSource,
                             [NotNull] iEventFactory pEventFactory)
        {
            if (pPluginSettings == null)
            {
                throw new ArgumentNullException("pPluginSettings");
            }
            if (pEventFactory == null)
            {
                throw new ArgumentNullException("pEventFactory");
            }

            Source = pDataSource;
            PluginSettings = pPluginSettings;
            _eventFactory = pEventFactory;

            _eventRecorder = null;
        }

        /// <summary>
        /// Dispose of resources.
        /// </summary>
        public virtual void Dispose()
        {
            Source.Dispose();
        }

        /// <summary>
        /// Records an exception that was unhandled by the current
        /// task processor.
        /// </summary>
        /// <param name="pException">The exception object.</param>
        public void UnhandledException(Exception pException)
        {
            _logger.Exception(pException);

            if (_eventRecorder == null)
            {
                return;
            }

            iEventObject eventObj = _eventFactory.Create(eEVENT_SEVERITY.UNHANDLED, pException);
            _eventRecorder.Add(eventObj);
        }

        /// <summary>
        /// Records an exception in the current task as
        /// being handled, but was thrown by the executor
        /// of the task.
        /// </summary>
        /// <param name="pException">The exception object.</param>
        public void RecordException(Exception pException)
        {
            _logger.Exception(pException);

            if (_eventRecorder == null)
            {
                return;
            }

            iEventObject eventObj = _eventFactory.Create(eEVENT_SEVERITY.HANDLED, pException);
            _eventRecorder.Add(eventObj);
        }

        /// <summary>
        /// Records an event as an error in the event log.
        /// </summary>
        public void RecordEvent(string pType, string pDesc, string pMessage)
        {
            if (_eventRecorder == null)
            {
                return;
            }

            iEventObject eventObj = _eventFactory.Create(eEVENT_SEVERITY.HANDLED, pType, pDesc, pMessage);
            _eventRecorder.Add(eventObj);
        }

        /// <summary>
        /// Assigns an exception recorder to the current context.
        /// </summary>
        public void setEventRecorder(iEventRecorder pRecorder)
        {
            _eventRecorder = pRecorder;
        }
    }
}