using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Scan;
using Engine.Scan.Variable;

namespace NodeTest.Scan
{
    [TestClass()]
    public class DateFactoryTest : AbstractFactoryTest
    {
        /// <summary>
        /// Dates in different formats.
        /// </summary>
        private List<KeyValuePair<DateTime, string>> FormattedDates;

        public DateFactoryTest()
        {
            FormattedDates = new List<KeyValuePair<DateTime, string>>();
            List<DateTime> dates = new List<DateTime>();

            for (var m = 1; m <= 12; m++)
            {
                for (var d = 1; d <= 30; d = d + 10)
                {
                    for (var y = 1990; y < 2030; y = y + 5)
                    {
                        dates.Add(new DateTime(y, m, d));
                    }
                }
            }

            foreach (DateTime d in dates)
            {
                FormattedDates.Add(new KeyValuePair<DateTime, string>(d, d.ToString("MM/dd/yyyy")));
                FormattedDates.Add(new KeyValuePair<DateTime, string>(d, d.ToString("MM/dd/yy")));
                FormattedDates.Add(new KeyValuePair<DateTime, string>(d, d.ToString("MM-dd-yy")));
            }
        }

        protected override string GetValue(VariableMatch pMatch)
        {
            DateMatch dm = pMatch as DateMatch;
            return dm.Value.ToString();
        }

        [TestMethod()]
        public void Basic_Date_Match()
        {
            Regex regex = new Regex(DateFactory.RegExDateStr);

            string[] tokens = new string[] { @"\", "/", "-", ".", " " };
            string str;

            foreach (string t in tokens)
            {
                for (var m = 1; m <= 12; m++)
                {
                    for (var d = 1; d <= 31; d++)
                    {
                        // test is true
                        str = string.Format("{1:00}{0}{2:00}{0}2000", t, m, d);
                        Assert.IsTrue(regex.IsMatch(str), str);
                        str = string.Format("{1:00}{0}{2:00}{0}11", t, m, d);
                        Assert.IsTrue(regex.IsMatch(str), str);

                        // test is false
                        str = string.Format("{1:00}x{2:00}x11", t, m, d);
                        Assert.IsFalse(regex.IsMatch(str), str);
                    }

                    // test is false
                    str = string.Format("31{0}{1:00}{0}2000", t, m);
                    Assert.IsFalse(regex.IsMatch(str), str);
                }
            }

            Assert.IsFalse(regex.IsMatch("00/00/00"));
            Assert.IsFalse(regex.IsMatch("00/12/2010"));
        }

        [TestMethod()]
        public void Date_Factory_Test()
        {
            DateFactory factory = new DateFactory("date", DateFactory.Dates, false);

            foreach (KeyValuePair<DateTime, string> pair in FormattedDates)
            {
                MatchParagraphs<DateTime>(factory, pair.Value, pair.Key, true);
            }
        }
    }
}
