using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Common.Annotations;
using Common.Events;
using GemsLogger;
using Jobs;
using Jobs.Exceptions;
using Jobs.Plugins;
using Jobs.Reports;
using Node.Actions;
using Node.Controls;
using Node.Properties;
using Node.Themes;

namespace Node
{
    /// <summary>
    /// The main application window shown when the tray icon is clicked.
    /// </summary>
    internal sealed partial class Main : Form, iMainFrmService
    {
        /// <summary>
        /// The ID for the Node.
        /// </summary>
        private static readonly Guid _guid = new Guid("0856F264-2629-45C0-99F4-20C151850584");

        /// <summary>
        /// The logger output.
        /// </summary>
        private static readonly Logger _logger = Logger.Create(typeof (Main));

        /// <summary>
        /// The UI command handlers.
        /// </summary>
        private readonly iActionService _actionService;

        /// <summary>
        /// Handles notification and changes of selected job.
        /// </summary>
        private readonly iActiveJobService _activeJobService;

        /// <summary>
        /// The job engine.
        /// </summary>
        private readonly iEngineService _engineService;

        /// <summary>
        /// The job service.
        /// </summary>
        private readonly iJobService _jobService;

        /// <summary>
        /// Displays details (the log) for a selected job.
        /// </summary>
        private readonly JobsViewStack _jobsViewStack;

        /// <summary>
        /// Reference to the top level options storage. Used to display the options editor dialog.
        /// </summary>
        private readonly iPluginStorage _storage;

        /// <summary>
        /// Branding information
        /// </summary>
        private readonly iAppTheme _theme;

        /// <summary>
        /// True to exit the application when all jobs finish.
        /// </summary>
        private bool _exiting;

        /// <summary>
        /// called to verify the reason for closing the window.
        /// </summary>
        /// <param name="pSender"></param>
        /// <param name="pEventArgs"></param>
        private void BeforeClose(object pSender, FormClosingEventArgs pEventArgs)
        {
            NodeSettings settings = (NodeSettings)_storage.Find(_guid);
            if (pEventArgs.CloseReason == CloseReason.UserClosing && settings.MinimizeOnClose)
            {
                if (settings.HideOnMinimize)
                {
                    _actionService.Trigger("Main.Hide");
                }
                else
                {
                    WindowState = FormWindowState.Minimized;
                }
                pEventArgs.Cancel = true;
                return;
            }
            _actionService.Trigger("Main.Shutdown");
        }

        /// <summary>
        /// Causes the application to exit.
        /// </summary>
        void iMainFrmService.Exit()
        {
            FireEvents.Invoke(this, ()=>
                                    {
                                        Hide();
                                        trayIcon.Visible = false;
                                        Environment.Exit(0);
                                    });
        }

        void iMainFrmService.Hide()
        {
            Hide();
            trayIcon.ShowBalloonTip(
                250,
                "Background",
                string.Format("{0} is still running in the background.", _theme.Title),
                ToolTipIcon.Info);
        }

        /// <summary>
        /// Associates UI events with command handlers.
        /// </summary>
        void iMainFrmService.Init()
        {
            _actionService.Register(menuShutdown);
            _actionService.Register(menuShutdown2);

            _actionService.Register(menuExit);
            _actionService.Register(menuExit2);
            _actionService.Register(menuOptions);
            _actionService.Register(menuClearErrors);

            _actionService.Register(menuJobClearErrors);
            _actionService.Register(menuJobSuspend);
            _actionService.Register(menuJobResume);
            _actionService.Register(menuJobStop);

            _actionService.Register(menuTaskClearErrors);

            _actionService.Register(menuSuspendAll);
            _actionService.Register(menuResumeAll);
            _actionService.Register(menuStopAll);

            trayIcon.Click += (pSender, pArgs)=>_actionService.Trigger("Main.Show");
        }

        void iMainFrmService.Show()
        {
            Show();
            Activate();
            WindowState = FormWindowState.Normal;

            Rectangle area = Screen.PrimaryScreen.WorkingArea;
            ClientSize = new Size((int)(area.Width * 0.66), (int)(area.Height * 0.66));
            DesktopLocation = new Point(area.Right - Width - 8, area.Bottom - Height - 8);
        }

        /// <summary>
        /// Application should exit as soon as possible.
        /// </summary>
        void iMainFrmService.Shutdown()
        {
            trayIcon.ShowBalloonTip(0, "Shutting Down",
                string.Format("{0} will shutdown once all jobs are finished.", _theme.Title),
                ToolTipIcon.Info);

            _exiting = true;

            if (_jobService.getRunning() == 0)
            {
                Application.Exit();
            }
            else
            {
                _engineService.Stop();
            }
        }

        /// <summary>
        /// The main form.
        /// </summary>
        Form iMainFrmService.getMainApplication()
        {
            return this;
        }

        /// <summary>
        /// </summary>
        private void onJobError(Guid pJobID, Exception pError)
        {
            FireEvents.Invoke(this, ()=>_logger.Error(pError.Message));
        }

        /// <summary>
        /// Removes logging from the work thread.
        /// </summary>
        private void onJobFinish(Guid pJobID)
        {
            FireEvents.Invoke(this, ()=>
                                    {
                                        if (_exiting && _jobService.getRunning() == 0)
                                        {
                                            Application.Exit();
                                        }
                                    });
        }

        /// <summary>
        /// Triggers saving the storage when the menu states are changed.
        /// </summary>
        /// <param name="pSender"></param>
        /// <param name="pEventArgs"></param>
        private void onMenuCheckChanged(object pSender, EventArgs pEventArgs)
        {
            _storage.Save();

            NodeSettings settings = (NodeSettings)_storage.Find(_guid);
            TopMost = settings.StayOnTop;
        }

        /// <summary>
        /// Hide the form when minimized.
        /// </summary>
        /// <param name="pSender"></param>
        /// <param name="pEventArgs"></param>
        private void onResized(object pSender, EventArgs pEventArgs)
        {
            if (!_engineService.isLoaded())
            {
                return;
            }
            NodeSettings settings = (NodeSettings)_storage.Find(_guid);
            if (WindowState == FormWindowState.Minimized && settings.HideOnMinimize)
            {
                _actionService.Trigger("Main.Hide");
            }
        }

        /// <summary>
        /// When a job is selected in the top browser.
        /// </summary>
        private void onSelectedJobChanged(Guid pJobID)
        {
            // set this job as the last active
            NodeSettings settings = (NodeSettings)_storage.Find(_guid);
            iJobReport jobReport = _jobService.getJobReport(pJobID, false);
            settings.JobCode = jobReport == null ? "" : jobReport.Code;
            _storage.Save();
        }

        /// <summary>
        /// Tell the PanelBrowser that the form is no longer visible, and
        /// it can stop using a timer to update it's UI.
        /// </summary>
        private void onVisibleChanged(object pSender, EventArgs pEventArgs)
        {
            _jobsViewStack.setUpdateTabs(Visible);
        }

        /// <summary>
        /// Called when the form is first loaded.
        /// </summary>
        private void onWindowLoad(object pSender, EventArgs e)
        {
            if (Settings.Default.start_open)
            {
                _actionService.Trigger("Main.Show");
            }
            else
            {
                Show();
                Hide();
            }

            _jobService.JobFinish += onJobFinish;
            _jobService.JobError += onJobError;

            try
            {
                // configure the application directory
                string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                // start the Engine
                _engineService.Load();
                _storage.Store(_guid, new NodeSettings());
                if (!_storage.Load(directory, "storage"))
                {
                    _logger.Error("Failed to load settings.");
                }
                _engineService.Start();
            }
            catch (JobException ex)
            {
                _logger.Exception(ex);
            }

            NodeSettings settings = (NodeSettings)_storage.Find(_guid);
            settings.bindMenu(menuStayOnTop, "StayOnTop");
            settings.bindMenu(menuMinimizeOnClose, "MinimizeOnClose");
            settings.bindMenu(menuHideMinimized, "HideOnMinimize");

            menuStayOnTop.Click += onMenuCheckChanged;
            menuMinimizeOnClose.Click += onMenuCheckChanged;
            menuHideMinimized.Click += onMenuCheckChanged;

            _logger.Fine("Selecting: {0}", settings.JobCode);

            Guid selected = (from guid in _jobService.getJobIDs()
                             where _jobService.getJobReport(guid, false).Code == settings.JobCode
                             select guid).FirstOrDefault();
            if (selected != default(Guid))
            {
                _activeJobService.setActiveJob(selected);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Main([NotNull] iActionService pActionService,
                    [NotNull] iAppTheme pAppTheme,
                    [NotNull] iEngineService pEngineService,
                    [NotNull] iJobService pJobService,
                    [NotNull] iPluginStorage pStorage,
                    [NotNull] iActiveJobService pActiveJobService,
                    [NotNull] iJobsView pJobsView)
        {
            if (pActionService == null)
            {
                throw new ArgumentNullException("pActionService");
            }
            if (pAppTheme == null)
            {
                throw new ArgumentNullException("pAppTheme");
            }
            if (pEngineService == null)
            {
                throw new ArgumentNullException("pEngineService");
            }
            if (pJobService == null)
            {
                throw new ArgumentNullException("pJobService");
            }
            if (pStorage == null)
            {
                throw new ArgumentNullException("pStorage");
            }
            if (pActiveJobService == null)
            {
                throw new ArgumentNullException("pActiveJobService");
            }
            if (pJobsView == null)
            {
                throw new ArgumentNullException("pJobsView");
            }

            InitializeComponent();

            _theme = pAppTheme;
            _engineService = pEngineService;
            _jobService = pJobService;
            _storage = pStorage;
            _activeJobService = pActiveJobService;
            _actionService = pActionService;

            pJobsView.setJobMenu(menuJob);
            pJobsView.setTaskMenu(menuTask);
            Control reportCtrl = pJobsView.getControl();
            reportCtrl.Dock = DockStyle.Fill;
            splitter.Panel1.Controls.Add(reportCtrl);

            _jobsViewStack = new JobsViewStack(pActiveJobService, _jobService)
                             {
                                 Dock = DockStyle.Fill,
                                 TabIndex = 2
                             };
            splitter.Panel2.Controls.Add(_jobsViewStack);

            pActiveJobService.JobChanged += onSelectedJobChanged;

            trayIcon.Text = _theme.Title;
            trayIcon.Icon = _theme.Icon;
            Text = _theme.Title;
            Icon = _theme.Icon;
        }
    }
}