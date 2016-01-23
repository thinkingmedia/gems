using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Scan;
using Engine.Scan.Variable;

namespace NodeTest.Scan
{
    [TestClass()]
    public class StringFactoryTest : AbstractFactoryTest
    {
        private string[] InvoiceNumbers = new string[] { "9349303", "393-494034", "39438234", "3249298AL", "RC32944-38" };
        private string[] BadInvoiceNumbers = new string[] { "ASD9338ASD", "asd128239", "210393483482384389323", "123342ABD324823", "1-1-1-1" };

        protected override string GetValue(VariableMatch pMatch)
        {
            StringMatch match = pMatch as StringMatch;
            return match.Value;
        }

        [TestMethod()]
        public void Test_Invoice_Numbers()
        {
            string str;
            StringFactory factory = new StringFactory(StringFactory.RegExInvoiceStr, "string", StringFactory.Invoices, true);

            foreach (string prefix in StringFactory.Invoices)
            {
                foreach (string invoice in InvoiceNumbers)
                {
                    str = string.Format("{0}: {1}", prefix, invoice);
                    MatchParagraphs<string>(factory, str, invoice, true);
                    str = string.Format("{0}", invoice);
                    MatchParagraphs<string>(factory, str, invoice, false);
                }

                foreach (string invoice in BadInvoiceNumbers)
                {
                    str = string.Format("{0}: {1}", prefix, invoice);
                    MatchParagraphs<string>(factory, str, invoice, false);
                    str = string.Format("{0}", invoice);
                    MatchParagraphs<string>(factory, str, invoice, false);
                }
            }
        }

        [TestMethod()]
        public void Test_Credit_Cards()
        {
            string str;
            StringFactory factory = new StringFactory(StringFactory.RegExPaymentStr, "string", StringFactory.Payments, false);

            foreach (string prefix in StringFactory.Payments)
            {
                foreach (string card in StringFactory.CreditCards)
                {
                    // must pass
                    str = string.Format("{0}: {1}", prefix, card);
                    MatchParagraphs<string>(factory, str, card, true);
                    str = string.Format("{0}", card);
                    MatchParagraphs<string>(factory, str, card, true);
                    str = string.Format("{0}: {1}", prefix, card.ToUpper());
                    MatchParagraphs<string>(factory, str, card.ToUpper(), true);
                    str = string.Format("{0}", card.ToUpper());
                    MatchParagraphs<string>(factory, str, card.ToUpper(), true);

                    // must fail
                    str = string.Format("{0}: {1}", prefix, card.ToLower());
                    MatchParagraphs<string>(factory, str, card.ToLower(), false);
                    str = string.Format("{0}", card.ToLower());
                    MatchParagraphs<string>(factory, str, card.ToLower(), false);
                }
            }
        }
    }
}
