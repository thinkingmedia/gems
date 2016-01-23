using System;
using System.Diagnostics;
using Audit.Logger;

namespace Audit.Writer
{
    /// <summary>
    /// Handles writing to the audit trail for a single post.
    /// You can pass this object to a function without the
    /// function having to worry about which post it is
    /// logging about.
    /// </summary>
    internal class AuditWriter : iAuditWriter
    {
        /// <summary>
        /// The owner ID.
        /// </summary>
        private readonly UInt32 _id;

        /// <summary>
        /// The logger object.
        /// </summary>
        private readonly AuditStorage _storage;

        /// <summary>
        /// Constructor
        /// </summary>
        public AuditWriter(UInt32 pId, AuditStorage pStorage)
        {
            _id = pId;
            _storage = pStorage;
        }

        /// <summary>
        /// The record ID that owns this log.
        /// </summary>
        public uint getID()
        {
            return _id;
        }

        /// <summary>
        /// Writes a line of formatted text to the log.
        /// </summary>
        public void WriteLine(string pStr, params object[] pArgs)
        {
            string prefix = "info";

            // gets the name of the calling class
            StackTrace stack = new StackTrace();
            StackFrame[] frames = stack.GetFrames();
            if (frames != null)
            {
                Type type = frames[1].GetMethod().DeclaringType;
                if (type != null)
                {
                    prefix = type.Name;
                }
            }

            string msg = string.Format(pStr, pArgs);
            msg = string.Format("{0}: [{1}] {2}", DateTime.Now.ToString("yy-MM-dd hh:mm tt"), prefix, msg);

            _storage.WriteLine(_id, msg);
        }

        /// <summary>
        /// Writes a heading to the log.
        /// </summary>
        public void Heading(string pStr, params object[] pArgs)
        {
            _storage.WriteLine(_id, "");
            _storage.WriteLine(_id, string.Format("### {0} ###", pStr), pArgs);
            _storage.WriteLine(_id, "");
        }
    }
}