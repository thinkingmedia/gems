using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Scan;
using Engine.Scan.Variable;

namespace NodeTest.Scan
{
    /// <summary>
    ///This is a test class for VariableRuleTest and is intended
    ///to contain all VariableRuleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CurrencyFactoryTest : AbstractFactoryTest
    {
        private static string[] GoodAmounts;

        private static double[] DoubleAmounts = new double[] { 
            0.00, 1.12, 2.32, 3.45, 4.56, 5.65,
            10.33, 24.32, 24.34, 23.45, 94.34, 39.23,
            219.04, 495.03, 940.03, 294.30, 292.00,
            1249.33, 3940.40, 3040.20, 4950.30, 4594.20,
            1203.40, 9495.30, 3059.00, 6495.20, 1304.30,
            10,203.04, 40495.05, 30950.00,
            1203102.30, 0.00 };

        private static string[] BadAmounts = new string[] { 
            "010.23", "203.", "593", "00.00", 
            "2A9.30", "O.12", "4.394.32", "2.405",
//            "%234.40", "^23.23", "@343.34.234.21%"
        };

        private void MatchValues(CurrencyFactory pFactory, string[] pValues, string[] pPrefixes, bool pMatch)
        {
            foreach (string value in pValues)
            {
                MatchParagraphs<string>(pFactory, string.Format("${0}", value), value, pMatch);
                MatchParagraphs<string>(pFactory, string.Format("$ {0}", value), value, pMatch);
                MatchParagraphs<string>(pFactory, string.Format("{0}", value), value, pMatch);
                foreach (string prefix in pPrefixes)
                {
                    foreach (string token in new string[] { ":", "#", ">", "-" })
                    {
                        MatchParagraphs<string>(pFactory, string.Format("{0}{1} ${2}", prefix, token, value), value, pMatch);
                        MatchParagraphs<string>(pFactory, string.Format("{0}{1} {2}", prefix, token, value), value, pMatch);
                        MatchParagraphs<string>(pFactory, string.Format("{0}{1}    ${2}", prefix, token, value), value, pMatch);
                        MatchParagraphs<string>(pFactory, string.Format("{0}{1}       {2}", prefix, token, value), value, pMatch);
                    }
                    MatchParagraphs<string>(pFactory, string.Format("{0} ${1}", prefix, value), value, pMatch);
                    MatchParagraphs<string>(pFactory, string.Format("{0} {1}", prefix, value), value, pMatch);
                    MatchParagraphs<string>(pFactory, string.Format("{0}    ${1}", prefix, value), value, pMatch);
                    MatchParagraphs<string>(pFactory, string.Format("{0}        {1}", prefix, value), value, pMatch);
                }
            }
        }

        public CurrencyFactoryTest()
        {
            // build a long list of possibly valid currency values.
            List<string> s = new List<string>();
            foreach (double d in CurrencyFactoryTest.DoubleAmounts)
            {
                s.Add(string.Format("{0:0.00}", d));
                s.Add(string.Format("-{0:0.00}", d));
                if (d >= 10.0)
                {
                    s.Add(string.Format("{0:0,0.00}", d));
                    s.Add(string.Format("-{0:0,0.00}", d));
                }
            }
            CurrencyFactoryTest.GoodAmounts = s.ToArray();
        }

        protected override string GetValue(VariableMatch pMatch)
        {
            CurrencyMatch match = pMatch as CurrencyMatch;
            return match.Value;
        }

        /// <summary>
        /// Test the matching numeric values.
        /// </summary>
        [TestMethod()]
        public void Test_Numbers()
        {
            Regex regex = new Regex(CurrencyFactory.RegExNumericStr);
            foreach (string value in GoodAmounts)
            {
                Match m = regex.Match(value);
                Assert.IsTrue(m.Groups["var"].Success, value);
                Assert.AreEqual(value, m.Groups["var"].Value);
            }
            foreach (string value in GoodAmounts)
            {
                string svalue = "$" + value;
                Match m = regex.Match(svalue);
                Assert.IsTrue(m.Groups["var"].Success, value);
                Assert.AreEqual(value, m.Groups["var"].Value);
            }

            foreach (string value in BadAmounts)
            {
                Match m = regex.Match(value);
                Assert.IsFalse(m.Groups["var"].Success, value);
            }
        }

        [TestMethod()]
        public void Test_Prefix_Match()
        {
            List<string> prefixes = new List<string>();
            prefixes.AddRange(CurrencyFactory.Totals);
            prefixes.AddRange(CurrencyFactory.Taxes);

            string expression = VariableMatch.Prefix(prefixes.ToArray());

            Regex regex = new Regex(expression);

            string[] phrases = new string[]{
                "{0}",
                "The big red fox jumped over {0} to the other side.",
                "sdo383 erbsdf 8 28 fsdb {0} sadnsad, sad.",
                "{0} sdahsadhk sad sadkhsa.",
                "sadkjsadh 8ds sbsa {0}"
            };

            foreach (string phrase in phrases)
            {
                foreach (string prefix in CurrencyFactory.Totals)
                {
                    string s = string.Format(phrase, prefix);
                    Match m = regex.Match(s);
                    Assert.AreEqual(prefix, m.Groups["prefix"].Value, true, s);
                }
            }
        }

        /// <summary>
        ///A test for Process
        ///</summary>
        [TestMethod()]
        public void The_Big_Test()
        {
            CurrencyFactory factory = new CurrencyFactory("Amount", CurrencyFactory.Totals, false);
            MatchValues(factory, GoodAmounts, CurrencyFactory.Totals, true);
            MatchValues(factory, BadAmounts, CurrencyFactory.Totals, false);

            factory = new CurrencyFactory("Tax", CurrencyFactory.Taxes, false);
            MatchValues(factory, GoodAmounts, CurrencyFactory.Taxes, true);
            MatchValues(factory, BadAmounts, CurrencyFactory.Taxes, false);
        }
    }
}
