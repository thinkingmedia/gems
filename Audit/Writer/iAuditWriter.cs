using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Audit.Writer
{
    /// <summary>
    /// Methods to write to a audit log for a given record.
    /// </summary>
    public interface iAuditWriter
    {
        /// <summary>
        /// The record ID that owns this log.
        /// </summary>
        UInt32 getID();

        /// <summary>
        /// Writes a line of formatted text to the log.
        /// </summary>
        void WriteLine(string pStr, params object[] pArgs);

        /// <summary>
        /// Writes a heading to the log.
        /// </summary>
        void Heading(string pStr, params object[] pArgs);
    }
}
