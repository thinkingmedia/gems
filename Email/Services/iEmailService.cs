using System.Net.Mail;

namespace Gems.Email.Services
{
    public interface iEmailService
    {
        /// <summary>
        /// Starts a new mail message.
        /// </summary>
        /// <param name="pFrom">The sender email address.</param>
        /// <param name="pReplyTo">The reply email address.</param>
        /// <param name="pSubject">The subject line</param>
        /// <param name="pTextBody">Contents for the plain text version, or Null.</param>
        /// <param name="pHtmlBody">Contents for the HTML version, or Null.</param>
        void Start(MailAddress pFrom, MailAddress pReplyTo, string pSubject, string pTextBody, string pHtmlBody);

        /// <summary>
        /// Adds to the sender list.
        /// </summary>
        void To(MailAddress pWho);

        /// <summary>
        /// Adds to the CC list.
        /// </summary>
        void Cc(MailAddress pWho);

        /// <summary>
        /// Adds to the BCC list.
        /// </summary>
        void Bcc(MailAddress pWho);

        /// <summary>
        /// Sends the email.
        /// </summary>
        void Send();
    }
}