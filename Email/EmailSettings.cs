using System.ComponentModel;
using Jobs.Plugins;

namespace Gems.Email
{
    public class EmailSettings : PluginSettings
    {
        [Category("Email")]
        [Description("The hour in 24 hour time to send each day.")]
        [DefaultValue(3)]
        public int Hour { get; set; }

        [Category("Email")]
        [Description("The name of the person sending the email.")]
        [DefaultValue("Mathew Foscarini")]
        public string AdminName { get; set; }

        [Category("Email")]
        [Description("The email address of the administrator.")]
        [DefaultValue("mathew@thinkingmedia.ca")]
        public string AdminEmail { get; set; }

        [Category("Email")]
        [Description("The subject line for the daily report.")]
        [DefaultValue("Daily Report")]
        public string Subject { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailSettings()
            : base("Email", 1)
        {
            Hour = 3;
            AdminEmail = "mathew@thinkingmedia.ca";
            AdminName = "Mathew Foscarini";
            Subject = "Daily Report";
        }
    }
}