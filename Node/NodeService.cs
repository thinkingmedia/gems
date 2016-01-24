using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using GemsLogger;
using GemsLogger.Formatters;
using GemsLogger.Writers;
using Jobs;
using Jobs.Reports;
using Node.Actions;
using Node.Actions.Jobs;
using Node.Actions.Main;
using Node.Themes;
using StructureMap;

namespace Node
{
    /// <summary>
    /// Implements the launcher interface.
    /// </summary>
    internal sealed class NodeService : iNodeService
    {
        /// <summary>
        /// Logging
        /// </summary>
        private static readonly Logger _logger = Logger.Create(typeof (NodeService));

        /// <summary>
        /// The theme for the application.
        /// </summary>
        private readonly iAppTheme _theme;

        /// <summary>
        /// Configures the logging of error messages.
        /// </summary>
        private static void CreateErrorLogger()
        {
            string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            ArchiveWriter archiveWriter = new ArchiveWriter(directory + @"\logs", "error");
            LevelWriter levelWriter = new LevelWriter(archiveWriter, new[] {Logger.eLEVEL.ERROR});
            FormatWriter formatWriter = new FormatWriter(levelWriter, new DetailFormat());

            Logger.Add(formatWriter);
        }

        /// <summary>
        /// Configures the logging of messages by thread.
        /// </summary>
        private static void CreateThreadLogger(string pName, int pThreadID)
        {
            string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            ArchiveWriter archiveWriter = new ArchiveWriter(directory + @"\logs", pName.ToLower().Replace(' ','-'));
            ThreadWriter threadWriter = new ThreadWriter(archiveWriter, pThreadID);
            FormatWriter formatWriter = new FormatWriter(threadWriter, new DetailFormat());

            Logger.Add(formatWriter);
        }

        /// <summary>
        /// Configures the logger.
        /// </summary>
        private void ConfigureLogger(iJobService pJobService)
        {
            DetailFormat detail = new DetailFormat();
            DetailFormat.Register("main");

            CreateThreadLogger("main", Thread.CurrentThread.ManagedThreadId);
            CreateErrorLogger();

            _logger.Fine(new String('*', 80));
            _logger.Fine("{0} Started", _theme.Title);
            _logger.Fine(new String('*', 80));

            // output each job to it's own log file.
            pJobService.JobStart += pGuid=>
                                    {
                                        iJobReport jobReport = pJobService.getJobReport(pGuid, false);
                                        CreateThreadLogger(string.Format("{0}.{1}", jobReport.Plugin, jobReport.Code), jobReport.ThreadID);
                                    };
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public NodeService(iAppTheme pTheme)
        {
            if (pTheme == null)
            {
                throw new ArgumentNullException("pTheme");
            }

            _theme = pTheme;

            iActionService actionService = ObjectFactory.GetInstance<iActionService>();
            iMainFrmService mainFrmService = ObjectFactory.GetInstance<iMainFrmService>();

            actionService.Register(new Exit());
            actionService.Register(new Shutdown());
            actionService.Register(new ShowMain());
            actionService.Register(new HideMain());
            actionService.Register(new EditOptions());
            actionService.Register(new ClearErrors());
            actionService.Register(new Stop());
            actionService.Register(new Resume());
            actionService.Register(new Suspend());

            ConfigureLogger(ObjectFactory.GetInstance<iJobService>());

            mainFrmService.Init();
        }

        /// <summary>
        /// The main application window.
        /// </summary>
        /// <returns></returns>
        public Form getMainWindow()
        {
            return ObjectFactory.GetInstance<iMainFrmService>() as Form;
        }

        /// <summary>
        /// The application theme
        /// </summary>
        /// <returns></returns>
        public iAppTheme getTheme()
        {
            return _theme;
        }
    }
}