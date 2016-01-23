using Gems.Email.Services;
using Gems.Email.Tasks;
using StructureMap;

namespace Gems.Email
{
    /// <summary>
    /// Handles dependency injection.
    /// </summary>
    public static class Dependencies
    {
        public static void Bootstrap(IContainer pContainer)
        {
            pContainer.Configure(pExp=>
                                 {
                                     pExp.ForSingletonOf<iMailingListService>().Use<MailingListService>();
                                     pExp.ForSingletonOf<iEmailService>().Use<EmailService>();
                                     pExp.For<EmailTask>().Use<EmailTask>();
                                 });
        }
    }
}