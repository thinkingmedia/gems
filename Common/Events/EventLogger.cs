using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using Common.Components;
using GemsLogger;

namespace Common.Events
{
    /// <summary>
    /// Handles the logging of important application events.
    /// </summary>
    public class EventLogger : iEventLogger, iEventWatcher
    {
        private static readonly Logger _logger = Logger.Create(typeof(EventLogger));

        /// <summary>
        /// The types of events that can be logged.
        /// </summary>
        public enum EventType
        {
            INFO,
            WARNING,
            ERROR,
            UNKNOWN
        }

        /// <summary>
        /// Represents an event record.
        /// </summary>
        public class EventRecord
        {
            /// <summary>
            /// The event type.
            /// </summary>
            public readonly EventType Type;

            /// <summary>
            /// When the event happen.
            /// </summary>
            public DateTime When;

            /// <summary>
            /// A description of the event.
            /// </summary>
            public string Desc;

            /// <summary>
            /// Has this event been e-mailed.
            /// </summary>
            public bool EMailed;

            /// <summary>
            /// Constructor.
            /// </summary>
            public EventRecord(EventType pType, string pDesc)
            {
                Type = pType;
                When = DateTime.Now;
                Desc = pDesc;
                EMailed = false;
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            public EventRecord(string pStr)
            {
                string[] parts = pStr.Split(',');

                Type = EventType.UNKNOWN;
                switch (parts[0])
                {
                    case "INFO":
                        Type = EventType.INFO;
                        break;
                    case "WARNING":
                        Type = EventType.WARNING;
                        break;
                    case "ERROR":
                        Type = EventType.ERROR;
                        break;
                }

                DateTime.TryParse(parts[1], out When);
                Desc = parts[2];
                bool.TryParse(parts[3], out EMailed);
            }

            /// <summary>
            /// Copy constructor.
            /// </summary>
            public EventRecord(EventRecord pRecord)
            {
                Type = pRecord.Type;
                When = pRecord.When;
                Desc = pRecord.Desc;
                EMailed = pRecord.EMailed;
            }

            /// <summary>
            /// Returns a serialized string of the record.
            /// </summary>
            public override string ToString()
            {
                Desc = Desc.Replace(',', '.');
                return String.Format("{0},{1},{2},{3}", Type, When, Desc, EMailed);
            }
        }

        /// <summary>
        /// Reports when a new event has been logged.
        /// </summary>
        public event EventHandler onNewEvent;

        /// <summary>
        /// Repeats Error and Warnings in the tray icon.
        /// </summary>
        private readonly TrayIcon _trayIcon;

        /// <summary>
        /// The _name of the file to start event history records.
        /// </summary>
        private readonly string _filename;

        /// <summary>
        /// How many days an event is kept before it is expired.
        /// </summary>
        private readonly int _expireDays;

        /// <summary>
        /// Used to provide safe thread access.
        /// </summary>
        private readonly object _threadSafe = new object();

        /// <summary>
        /// Should the event logger send e-mail alerts.
        /// </summary>
        private const bool _E_MAIL_ENABLED = true;

        /// <summary>
        /// Who e-mail will be sent to.
        /// </summary>
        private readonly string _eMailTo;

        /// <summary>
        /// Who the e-mail will be from.
        /// </summary>
        private readonly string _eMailFrom;

        /// <summary>
        /// The SMTP _server to use.
        /// </summary>
        private readonly string _eMailServer;

        /// <summary>
        /// The SMTP port to use.
        /// </summary>
        private const int _E_MAIL_PORT = 25;

        /// <summary>
        /// An optional username for sending e-mail (credentials).
        /// </summary>
        private readonly string _eMailUsername = null;

        /// <summary>
        /// If EMailUsername is set, then the _password must be set as well.
        /// </summary>
        private readonly string _eMailPassword = null;

        /// <summary>
        /// Prefix for all e-mail subjects.
        /// </summary>
        private readonly string _prefixSubject;

        /// <summary>
        /// Constructor
        /// </summary>
        public EventLogger(TrayIcon pTrayIcon, string pFilename, int pExpire, int pManifestInterval, string pEMailTo, string pEMailServer)
        {
            _trayIcon = pTrayIcon;
            _filename = pFilename;
            _expireDays = pExpire;
            _eMailTo = pEMailTo;
            _eMailServer = pEMailServer;

            _prefixSubject = "Event";
            _eMailFrom = "no-reply@nowhere.com";

            Load(false);

            // the time for sending a manifest e-mail.
            if (pManifestInterval != 0)
            {
                //new System.Threading.Timer(EMailManifest, null, 0, pManifestInterval);
            }
        }

        /// <summary>
        /// Appends new event records to the events file.
        /// </summary>
        private void Save(EventRecord pRecord)
        {
            lock (_threadSafe)
            {
                // send errors right away.
                if (pRecord.Type == EventType.ERROR)
                {
                    MailAlert(pRecord);
                }

                try
                {
                    TextWriter writer = new StreamWriter(_filename, true, System.Text.Encoding.UTF8);
                    writer.WriteLine(pRecord.ToString());
                    writer.Flush();
                    writer.Close();
                }
                catch (Exception e)
                {
                    _logger.Exception(e);
                }
            }
        }

        /// <summary>
        /// Loads all the events from a file.
        /// </summary>
        private List<EventRecord> Load(bool pMarkEmailed)
        {
            lock (_threadSafe)
            {
                List<EventRecord> records = new List<EventRecord>();

                TextWriter writer = null;
                TextReader reader = null;

                string tmpFile = Path.GetTempFileName();

                // only load if the file exists
                if (!File.Exists(_filename))
                {
                    return records;
                }
                try
                {
                    // Read in each event, and delete events who's date has expired. Write these
                    // events out to a temp file, then replace the event Log file with the
                    // updated Log file.

                    // any events older then this date will be deleted.
                    DateTime expired = DateTime.Now.AddDays(-_expireDays);

                    writer = new StreamWriter(tmpFile, false, Encoding.UTF8);
                    reader = new StreamReader(_filename, Encoding.UTF8);
                    string str;
                    while ((str = reader.ReadLine()) != null)
                    {
                        try
                        {
                            EventRecord record = new EventRecord(str);
                            records.Add(record);

                            if (record.When > expired)
                            {
                                if (pMarkEmailed)
                                {
                                    record = new EventRecord(record) { EMailed = true };
                                }
                                writer.WriteLine(record.ToString());
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.Exception(e);
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.Exception(e);
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }

                    if (writer != null)
                    {
                        writer.Flush();
                        writer.Close();
                    }

                    File.Delete(_filename);
                    File.Move(tmpFile, _filename);
                }

                return records;
            }
        }

        /// <summary>
        /// Sends an e-mail alert for errors.
        /// </summary>
        private void MailAlert(EventRecord pRecord)
        {
            string str = String.Format("[{0}] {1} - {2}\n", pRecord.When, pRecord.Type, pRecord.Desc);
            SendEMail(pRecord.Type.ToString(), str);
        }

        /// <summary>
        /// Sends an e-mail reporting all events that haven't been e-mail since the
        /// last manifest.
        /// </summary>
        private void MailManifest(object pState)
        {
            if (_E_MAIL_ENABLED)
            {
                List<EventRecord> records = Load(true);
                StringBuilder sb = new StringBuilder();
                int count = 0;
                foreach (EventRecord record in records)
                {
                    if (!record.EMailed)
                    {
                        string str = String.Format("[{0}] {1} - {2}\n", record.When.ToString(), record.Type.ToString(), record.Desc);
                        sb.Append(str);
                        count++;
                    }
                }
                if (count > 0)
                {
                    string subject = String.Format("Manifest ({0})", count);
                    SendEMail(subject, sb.ToString());
                }
            }
        }

        /// <summary>
        /// Sends an e-mail to the system admin.
        /// </summary>
        private bool SendEMail(string pSubject, string pBody)
        {
            try
            {
                StringBuilder sb = new StringBuilder(pBody);
                sb.Append("\n");
                sb.Append("********************************************************************\n");
                sb.Append("Please do not reply to this email. This is a system generated email.\n");
                sb.Append("********************************************************************\n");

                // Create mail message object
                MailMessage mail = new MailMessage();

                // support comma delimited list
                string[] recipients = _eMailTo.Split(',');
                foreach (string recip in recipients)
                {
                    mail.To.Add(recip);
                }

                mail.From = new MailAddress(_eMailFrom);
                mail.Subject = String.Format("{0} - {1}", _prefixSubject, pSubject);
                mail.Body = sb.ToString();

                SmtpClient smtp = new SmtpClient(_eMailServer, _E_MAIL_PORT);

                if (!string.IsNullOrEmpty(_eMailUsername))
                {
                    smtp.Credentials = new NetworkCredential(_eMailUsername, _eMailPassword);
                }

                smtp.Send(mail);
                return true;
            }
            catch (Exception e)
            {
                _logger.Exception(e);
            }

            return false;
        }

        /// <summary>
        /// Returns a list of X number of events. If amount is zero then all the
        /// events are returned.
        /// </summary>
        public IEnumerable<EventRecord> getEvents(int pAmount)
        {
            return Load(false);
        }

        /// <summary>
        /// Clears the event Log.
        /// </summary>
        public bool clear()
        {
            try
            {
                File.Delete(_filename);
            }
            catch (Exception e)
            {
                _logger.Exception(e);
                return false;
            }

            if (onNewEvent != null)
            {
                onNewEvent(this, EventArgs.Empty);
            }

            return true;
        }

        /// <summary>
        /// Logs an event.
        /// </summary>
        private void Log(EventType pType, string pMsg, params object[] pArgs)
        {
            pMsg = String.Format(pMsg, pArgs);

            EventRecord record = new EventRecord(pType, pMsg);

            if (pType == EventType.ERROR)
            {
                _logger.Error(pMsg);
                _trayIcon.alert(pMsg);
            }

            if (pType == EventType.WARNING)
            {
                _logger.Fine(pMsg);
                _trayIcon.warning(pMsg);
            }

            if (pType == EventType.INFO)
            {
                _logger.Fine(pMsg);
            }

            if (onNewEvent != null)
            {
                onNewEvent(this, EventArgs.Empty);
            }

            Save(record);
        }

        /// <summary>
        /// Reports a resource as connected.
        /// </summary>
        /// <param _name="who"></param>
        /// <param name="pWho"></param>
        public void Connected(string pWho)
        {
            Log(EventType.INFO, String.Format("{0} is connected.", pWho));
            _trayIcon.connected();
        }

        /// <summary>
        /// Reports a resource as disconnected.
        /// </summary>
        public void Disconnected(string pWho)
        {
            Log(EventType.WARNING, String.Format("{0} is disconnected.", pWho));
        }

        /// <summary>
        /// Creates an information event.
        /// </summary>
        public void Info(String pStr, params object[] pArgs)
        {
            Log(EventType.INFO, String.Format(pStr, pArgs));
        }

        /// <summary>
        /// Creates a warning event.
        /// </summary>
        public void Warning(String pStr, params object[] pArgs)
        {
            Log(EventType.WARNING, String.Format(pStr, pArgs));
        }

        /// <summary>
        /// Creates an Error event.
        /// </summary>
        public void Error(String pStr, params object[] pArgs)
        {
            Log(EventType.ERROR, String.Format(pStr, pArgs));
        }

        /// <summary>
        /// This code can send test e-mails.
        /// </summary>
        public bool canTestEMail()
        {
            return true;
        }

        public bool sendTestEMail()
        {
            return SendEMail("Test E-Mail", "The administrator requested that a test e-mail be sent.");
        }
    }
}
