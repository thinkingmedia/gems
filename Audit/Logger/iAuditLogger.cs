using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Audit.Writer;

namespace Audit.Logger
{
    /// <summary>
    /// Provides access to the log and to create loggers.
    /// </summary>
    public interface iAuditLogger
    {
        /// <summary>
        /// Creates a writer that saves entries for a parent record.
        /// </summary>
        iAuditWriter CreateWriter(UInt32 pOwnerID);
    }
}
