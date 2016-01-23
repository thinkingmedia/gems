using System;
using System.Collections.Generic;
using Audit.Logger;
using DataSources;
using DataSources.DataSource;

namespace Audit.Models
{
    /// <summary>
    /// Handles the storage of logs.
    /// </summary>
    public class Audit : Model
    {
        /// <summary>
        /// Updates the audit log for the current post.
        /// </summary>
        private void Append(UInt32 pID, string pLog)
        {
            // append the log to the existing log
            string log = ReadField<string>(pID, "log");
            log = log ?? "";
            log = log.Trim() + "\n" + pLog + "\n";
            SaveField(pID, "log", log);
        }

        /// <summary>
        /// Assigns an audit_id record to the owner model. Creates a new
        /// audit record if needed.
        /// </summary>
        /// <param name="pOwner">The owner model (Post or Board)</param>
        /// <param name="pOwnerId">ID of the owner.</param>
        /// <returns>The audit ID.</returns>
        private UInt32 getAuditID(Model pOwner, UInt32 pOwnerId)
        {
            uint auditID = pOwner.ReadField<UInt32>(pOwnerId, "audit_id");
            if (auditID != default(UInt32))
            {
                return auditID;
            }
            auditID = Create(new Record());
            pOwner.SaveField(pOwnerId, "audit_id", auditID);
            return auditID;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Audit(iDataSource pSource)
            : base(pSource)
        {
        }

        /// <summary>
        /// Saves the log to the audit table for each owner record.
        /// </summary>
        public void Save(Model pOwner, iAuditLogger pStorage)
        {
            AuditStorage storage = (AuditStorage)pStorage;
            IEnumerable<uint> ids = storage.getIds();
            foreach (UInt32 id in ids)
            {
                string log = storage.getLog(id);
                if (log != "")
                {
                    UInt32 auditID = getAuditID(pOwner, id);
                    Append(auditID, log);
                }
                storage.Clear(id);
            }
        }
    }
}