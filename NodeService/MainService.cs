using System.ServiceProcess;

namespace NodeService
{
    public partial class MainService : ServiceBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MainService()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Starts the service from the command line.
        /// </summary>
        public void DebugStart()
        {
            this.OnStart(null);
        }

        /// <summary>
        /// Stops the service from the command line.
        /// </summary>
        public void DebugStop()
        {
            this.OnStop();
        }

        /// <summary>
        /// Starts the service.
        /// </summary>
        protected override void OnStart(string[] args)
        {
            EngineManager.Instance().StartUp();
        }

        /// <summary>
        /// Stops the service.
        /// </summary>
        protected override void OnStop()
        {
            EngineManager.Instance().ShutDown();
        }
    }
}
