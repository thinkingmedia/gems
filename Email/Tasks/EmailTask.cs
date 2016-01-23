using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Common.Utils;
using Gems.Email.Services;
using Gems.Email.Tasks.Report;
using Jobs;
using Jobs.Context;
using Jobs.Reports;
using Jobs.Tasks.Events;
using MarkdownDeep;
using Nustache.Core;
using Task = Jobs.Tasks.Task;

namespace Gems.Email.Tasks
{
    public class EmailTask : Task
    {
        /// <summary>
        /// The service that sends out emails.
        /// </summary>
        private readonly iEmailService _emailService;

        /// <summary>
        /// Provides access to the running jobs.
        /// </summary>
        private readonly iJobService _jobService;

        /// <summary>
        /// The service that provides who gets emails
        /// </summary>
        private readonly iMailingListService _mailingList;

        /// <summary>
        /// The email template.
        /// </summary>
        private readonly Template _template;

        /// <summary>
        /// The markdown engine
        /// </summary>
        private readonly Markdown _markdown;

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailTask(iJobService pJobService, iEmailService pEmailService, iMailingListService pMailingList)
        {
            if (pJobService == null || pEmailService == null || pMailingList == null)
            {
                throw new NullReferenceException();
            }

            _jobService = pJobService;
            _emailService = pEmailService;
            _mailingList = pMailingList;

            Resources resources = new Resources(GetType(), "Email.Templates");
            string markdown = resources.ReadAsString("email.md");
            using (StringReader reader = new StringReader(markdown))
            {
                _template = new Template();
                _template.Load(reader);
            }

            _markdown = new Markdown { ExtraMode = true, SafeMode = false };
        }

        /// <summary>
        /// Called by the worker thread.
        /// </summary>
        public override void Execute(JobContext pContext, iEventRecorder pEventRecorder)
        {
            EmailSettings settings = (EmailSettings)pContext.PluginSettings;

/*
            IEnumerable<iJobReport> jobReports = from jobID in _jobService.getJobIDs()
                                                 select _jobService.getJobReport(jobID, true);

            IEnumerable<IGrouping<string, iJobReport>> plugins = from report in jobReports
                                                                 group report by report.Plugin
                                                                 into plugin
                                                                 select plugin;

            Report.Report data = new Report.Report(from plugin in plugins
                                                   select new Plugin(
                                                       plugin.Key,
                                                       from job in plugin
                                                       select new Job(
                                                           job.Name,
                                                           from t in job.Tasks
                                                           select new Report.Task(
                                                               t.Name,
                                                               from e in t.EventRecorder.getEvents()
                                                               select new Event(e.Message)
                                                               )
                                                           )
                                                       ));
*/

/*
            Dictionary<string, List<Dictionary<string, object>>> data = new Dictionary<string, List<Dictionary<string, object>>>();
            data.Add("Reports", new List<Dictionary<string, object>>());
            data["Reports"].Add(new Dictionary<string, object>());
            data["Reports"][0].Add("title", "This is a title");
            data["Reports"][0].Add("summary", "This is a summary");
*/

            DataReport report = new DataReport(new[]{"Name","When","Value"}) {Title = "This is a title", Summary = "This is a summary"};
            report.Tables[0].Add(new[] {"Mathew", "10/10/10", "1203"});

            DataReports reports = new DataReports();
            reports.Reports.Add(report);

            StringWriter outWriter = new StringWriter();
            _template.Render(reports, outWriter, null, null);

            string output = _markdown.Transform(outWriter.ToString());

            MailAddress from = new MailAddress(settings.AdminEmail, settings.AdminName);

            _emailService.Start(from, from, settings.Subject, null, output);
            foreach (MailAddress who in _mailingList.getMailingList())
            {
                _emailService.To(who);
            }
            _emailService.Send();
        }
    }
}