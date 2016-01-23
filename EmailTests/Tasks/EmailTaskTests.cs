using System;
using System.Linq;
using Gems.Email.Tasks;
using Gems.EmailTests.Mock.Context;
using Gems.EmailTests.Mock.Services;
using JobsTest.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gems.EmailTests.Tasks
{
    [TestClass]
    public class EmailTaskTests
    {
        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void Construct_1()
        {
            EmailTask task = new EmailTask(null, null, null);
        }

        [TestMethod]
        public void Execute_1()
        {
            MockJobService jobService = new MockJobService(MockJob.Create());
            MockEmailService emailService = new MockEmailService();
            MockMailingListService mailingList = new MockMailingListService();

            EmailTask task = new EmailTask(jobService, emailService, mailingList);
            task.Execute(MockEmailContextFactory.Create(), null);

            Assert.AreEqual(1, emailService.Emails.Count);
            Assert.AreEqual(1, emailService.Sent);

            string html = emailService.Emails.First().Html;
            Console.WriteLine(html);
        }
    }
}