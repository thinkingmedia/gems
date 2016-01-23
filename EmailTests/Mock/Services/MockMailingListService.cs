using System.Collections.Generic;
using System.Net.Mail;
using Gems.Email.Services;

namespace Gems.EmailTests.Mock.Services
{
    public class MockMailingListService : iMailingListService
    {
        public IEnumerable<MailAddress> getMailingList()
        {
            return new List<MailAddress>
                   {
                       new MailAddress("mathew@thinkingmedia.ca", "Mathew"),
                       new MailAddress("jobs@thinkingmedia.ca", "Jobs")
                   };
        }
    }
}