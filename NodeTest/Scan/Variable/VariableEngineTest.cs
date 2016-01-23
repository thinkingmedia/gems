using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Scan;
using Engine.Scan.Variable;

namespace NodeTest.Scan.Variable
{
    [TestClass()]
    public class VariableEngineTest
    {
        [TestMethod()]
        public void Test_Basic_Engine()
        {
            VariableEngine engine = new VariableEngine();
            engine.Add(new CurrencyFactory("Amount", CurrencyFactory.Totals, false));
            engine.MatchAll(1, "Amount: $123.45");
            CurrencyMatch match = (CurrencyMatch)engine.BestMatch("Amount");
            Assert.IsNotNull(match);
            Assert.AreEqual(123.45, match.Amount);
        }

        [TestMethod()]
        public void Test_Highest_Amount()
        {
            VariableEngine engine = new VariableEngine();
            engine.Add(new CurrencyFactory("Amount", CurrencyFactory.Totals, false));
            engine.MatchAll(1, "10.00");
            engine.MatchAll(1, "40.00");
            engine.MatchAll(1, "30.00");
            engine.MatchAll(1, "20.00");

            CurrencyMatch match = (CurrencyMatch)engine.BestMatch("Amount");
            Assert.IsNotNull(match);
            Assert.AreEqual(40.00, match.Amount);
        }

        [TestMethod()]
        public void Test_Highest_DollarSign()
        {
            VariableEngine engine = new VariableEngine();
            engine.Add(new CurrencyFactory("Amount", CurrencyFactory.Totals, false));
            engine.MatchAll(1, "$10.00");
            engine.MatchAll(1, "40.00");
            engine.MatchAll(1, "30.00");
            engine.MatchAll(1, "$20.00");

            CurrencyMatch match = (CurrencyMatch)engine.BestMatch("Amount");
            Assert.IsNotNull(match);
            Assert.AreEqual(20.00, match.Amount);
        }

        [TestMethod()]
        public void Test_Highest_Prefix()
        {
            VariableEngine engine = new VariableEngine();
            engine.Add(new CurrencyFactory("Amount", CurrencyFactory.Totals, false));
            engine.MatchAll(1, "Total: $10.00");
            engine.MatchAll(1, "40.00");
            engine.MatchAll(1, "30.00");
            engine.MatchAll(1, "$20.00");

            CurrencyMatch match = (CurrencyMatch)engine.BestMatch("Amount");
            Assert.IsNotNull(match);
            Assert.AreEqual(10.00, match.Amount);
        }
    }
}
