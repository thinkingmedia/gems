using Audit.Logger;

namespace Audit
{
    /// <summary>
    /// Use this to create a logger.
    /// </summary>
    public static class AuditFactory
    {
        /// <summary>
        /// Creates a logger that gives you access to a writer.
        /// </summary>
        public static iAuditLogger Create()
        {
            return new AuditStorage();
        }
    }
}