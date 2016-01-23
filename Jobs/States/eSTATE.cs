namespace Jobs.States
{
    /// <summary>
    /// Defines the state of the job.
    /// </summary>
    public enum eSTATE
    {
        /// <summary>
        /// When the job is created, but before the worker thread is started.
        /// </summary>
        NONE,

        /// <summary>
        /// The worker thread is waiting.
        /// </summary>
        IDLE,

        /// <summary>
        /// The worker thread is not performing any tasks.
        /// </summary>
        SUSPENDED,

        /// <summary>
        /// The worker thread is busy.
        /// </summary>
        BUSY,

        /// <summary>
        /// The worker thread has exited with errors.
        /// </summary>
        FAILED,

        /// <summary>
        /// The worker thread has exited.
        /// </summary>
        FINISHED
    }
}