using System.Windows.Forms;

namespace Node
{
    /// <summary>
    /// The main application services.
    /// </summary>
    internal interface iMainFrmService
    {
        /// <summary>
        /// Application should exit immediately.
        /// </summary>
        void Exit();

        /// <summary>
        /// Hide the main form.
        /// </summary>
        void Hide();

        /// <summary>
        /// Initialize the service.
        /// </summary>
        void Init();

        /// <summary>
        /// Show the main form.
        /// </summary>
        void Show();

        /// <summary>
        /// Application should exit as soon as possible.
        /// </summary>
        void Shutdown();

        /// <summary>
        /// The main form.
        /// </summary>
        Form getMainApplication();
    }
}