using System;
using System.Collections.Generic;
using System.Linq;
using Audit.Writer;

namespace Audit.Logger
{
    internal class AuditStorage : iAuditLogger
    {
        /// <summary>
        /// Logging
        /// </summary>
        private static readonly GemsLogger.Logger _logger = GemsLogger.Logger.Create(typeof(AuditStorage));

        /// <summary>
        /// Audit logs for each record that is processed.
        /// </summary>
        private readonly Dictionary<uint, List<string>> _auditTrail;

        /// <summary>
        /// Constructor
        /// </summary>
        public AuditStorage()
        {
            _auditTrail = new Dictionary<uint, List<string>>();
        }

        /// <summary>
        /// Creates a new writer for this logger.
        /// </summary>
        public iAuditWriter CreateWriter(uint pOwnerID)
        {
            return new AuditWriter(pOwnerID, this);
        }

        /// <summary>
        /// Clears the log for a record ID.
        /// </summary>
        public void Clear(UInt32 pId)
        {
            _auditTrail.Remove(pId);
        }

        /// <summary>
        /// Tasks can report audit messages about what they have
        /// done. These messages could be saved by a final task
        /// for later reviewing the work completed.
        /// </summary>
        public void WriteLine(UInt32 pId, string pStr, params object[] pArgs)
        {
            if (!_auditTrail.ContainsKey(pId))
            {
                _auditTrail.Add(pId, new List<string>());
            }

            string msg = string.Format(pStr, pArgs);
            _auditTrail[pId].Add(msg);
            _logger.Debug(msg);
        }

        /// <summary>
        /// Returns an array of the record IDs that have audit logs.
        /// </summary>
        public IEnumerable<uint> getIds()
        {
            return _auditTrail.Keys.ToList();
        }

        /// <summary>
        /// Returns the log for a post.
        /// </summary>
        public string getLog(UInt32 pId)
        {
            return _auditTrail.ContainsKey(pId) ? string.Join("\n", _auditTrail[pId]) : "";
        }
    }
}