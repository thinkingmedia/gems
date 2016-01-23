using System;
using System.Drawing;
using Jobs.Reports;
using Jobs.States;

namespace Node.Controls
{
    /// <summary>
    /// Handles the display style of display job info.
    /// </summary>
    public static class JobInfoTheme
    {
        public enum eTHEME_ELEMENT
        {
            STATE,
            TIMER,
            ERRORS
        }

        /// <summary>
        /// This is used to create the timer count down to when a
        /// job is going to run again, or how long it has been
        /// running.
        /// </summary>
        private static string AsDuration(DateTime pRelative, DateTime pWhen)
        {
            TimeSpan duration =
                TimeSpan.FromSeconds(
                    Math.Round(
                        pRelative.Subtract(pWhen).Duration().TotalSeconds
                        ));
            return duration.ToString("c");
        }

        /// <summary>
        /// Gets background color of an element.
        /// </summary>
        public static Color BackColor(eTHEME_ELEMENT pElement, iJobReport pReport)
        {
            switch (pElement)
            {
                case eTHEME_ELEMENT.STATE:
                    if (pReport.State == eSTATE.FAILED)
                    {
                        return Color.Red;
                    }
                    break;
                case eTHEME_ELEMENT.TIMER:
                    if(pReport.State == eSTATE.BUSY)
                    {
                        return Color.DarkGreen;
                    }
                    break;
                case eTHEME_ELEMENT.ERRORS:
                    if (pReport.Errors > 0)
                    {
                        return Color.Red;
                    }
                    break;
            }

            return Color.White;
        }

        /// <summary>
        /// The text for errors.
        /// </summary>
        public static string ErrorsText(iJobReport pReport)
        {
            return string.Format("{0}/{1}", pReport.Errors, pReport.MaxErrors);
        }

        /// <summary>
        /// Gets foreground color of an element.
        /// </summary>
        public static Color ForeColor(eTHEME_ELEMENT pElement, iJobReport pReport)
        {
            switch (pElement)
            {
                case eTHEME_ELEMENT.STATE:
                    switch (pReport.State)
                    {
                        case eSTATE.FAILED:
                            return Color.White;
                        case eSTATE.SUSPENDED:
                            return Color.DodgerBlue;
                        case eSTATE.IDLE:
                            return Color.DarkOrange;
                        case eSTATE.BUSY:
                            return Color.Green;
                    }
                    break;
                case eTHEME_ELEMENT.TIMER:
                    switch (pReport.State)
                    {
                        case eSTATE.BUSY:
                            return Color.White;
                    }
                    break;
                case eTHEME_ELEMENT.ERRORS:
                    if (pReport.Errors > 0)
                    {
                        return Color.White;
                    }
                    break;
            }

            return Color.Black;
        }

        /// <summary>
        /// The text for the job status.
        /// </summary>
        public static string StateText(iJobReport pReport)
        {
            return pReport.State.ToString().ToUpper();
        }

        /// <summary>
        /// The text to show for the job's timer.
        /// </summary>
        public static string TimerText(iJobReport pReport)
        {
            switch (pReport.State)
            {
                case eSTATE.BUSY:
                    return AsDuration(DateTime.Now, pReport.TimeStamp);
                case eSTATE.FAILED:
                case eSTATE.FINISHED:
                    return pReport.TimeStamp.ToString("g");
            }

            return AsDuration(pReport.TimeStamp, DateTime.Now);
        }
    }
}