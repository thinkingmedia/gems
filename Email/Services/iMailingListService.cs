using System.Collections.Generic;
using System.Net.Mail;

namespace Gems.Email.Services
{
    public interface iMailingListService
    {
        IEnumerable<MailAddress> getMailingList();
    }
}