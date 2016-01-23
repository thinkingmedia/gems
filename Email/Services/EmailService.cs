using System.Net.Mail;

namespace Gems.Email.Services
{
    public class EmailService : iEmailService
    {
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
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Adds to the sender list.
        /// </summary>
        public void To(MailAddress pWho)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Adds to the CC list.
        /// </summary>
        public void Cc(MailAddress pWho)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Adds to the BCC list.
        /// </summary>
        public void Bcc(MailAddress pWho)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        public void Send()
        {
            throw new System.NotImplementedException();
        }
    }
}