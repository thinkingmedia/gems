using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Gems.Email.Services;

namespace Gems.EmailTests.Mock.Services
{
    public class MockEmailService : iEmailService
    {
        public List<Email> Emails = new List<Email>();

        public int Sent;

        /// <summary>
        /// Starts a new mail message.
        /// </summary>
        /// <param name="pFrom">The sender email address.</param>
        /// <param name="pReplyTo">The reply email address.</param>
        /// <param name="pSubject">The subject line</param>
        /// <param name="pTextBody">Contents for the plain text version, or Null.</param>
        /// <param name="pHtmlBody">Contents for the HTML version, or Null.</param>
        public void Start(MailAddress pFrom, MailAddress pReplyTo, string pSubject, string pTextBody, string pHtmlBody)
        {
            Emails.Add(new Email
                       {
                           From = pFrom,
                           ReplyTo = pReplyTo,
                           Subject = pSubject,
                           Text = pTextBody,
                           Html = pHtmlBody
                       });
        }

        /// <summary>
        /// Adds to the sender list.
        /// </summary>
        public void To(MailAddress pWho)
        {
            Emails.Last().To.Add(pWho);
        }

        /// <summary>
        /// Adds to the CC list.
        /// </summary>
        public void Cc(MailAddress pWho)
        {
            Emails.Last().Cc.Add(pWho);
        }

        /// <summary>
        /// Adds to the BCC list.
        /// </summary>
        public void Bcc(MailAddress pWho)
        {
            Emails.Last().Bcc.Add(pWho);
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        public void Send()
        {
            Sent++;
        }

        public class Email
        {
            public List<MailAddress> Bcc = new List<MailAddress>();
            public List<MailAddress> Cc = new List<MailAddress>();
            public MailAddress From;
            public string Html;
            public MailAddress ReplyTo;
            public string Subject;
            public string Text;
            public List<MailAddress> To = new List<MailAddress>();
        }
    }
}