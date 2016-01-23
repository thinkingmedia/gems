using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Scan;

namespace NodeTest.Scan
{


    /// <summary>
    ///This is a test class for CurrencyMatchTest and is intended
    ///to contain all CurrencyMatchTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CurrencyMatchTest
    {
        private CurrencyMatch a, b;

        /// <summary>
        ///A test for Value
        ///</summary>
        [TestMethod()]
        public void Test_Are_Equal()
        {
            a = new CurrencyMatch(1, "100.00", null, false);
            b = new CurrencyMatch(2, "100.00", null, false);
            Assert.AreEqual(0, a.CompareTo(b));

            a = new CurrencyMatch(1, "100.00", "Total", false);
            b = new CurrencyMatch(2, "100.00", "Total", false);
            Assert.AreEqual(0, a.CompareTo(b));

            a = new CurrencyMatch(1, "100.00", null, true);
            b = new CurrencyMatch(2, "100.00", null, true);
            Assert.AreEqual(0, a.CompareTo(b));

            a = new CurrencyMatch(1, "100.00", "Total", true);
            b = new CurrencyMatch(2, "100.00", "Total", true);
            Assert.AreEqual(0, a.CompareTo(b));
        }

        [TestMethod()]
        public void Test_Less_Then()
        {
            a = new CurrencyMatch(1, "50.00", null, false);
            b = new CurrencyMatch(2, "100.00", null, false);
            Assert.AreEqual(-1, a.CompareTo(b));

            a = new CurrencyMatch(1, "50.00", null, false);
            b = new CurrencyMatch(2, "50.00", "Total", false);
            Assert.AreEqual(-1, a.CompareTo(b));

            a = new CurrencyMatch(1, "50.00", null, false);
            b = new CurrencyMatch(2, "50.00", null, true);
            Assert.AreEqual(-1, a.CompareTo(b));

            a = new CurrencyMatch(1, "50.00", null, true);
            b = new CurrencyMatch(2, "50.00", "Total", true);
            Assert.AreEqual(-1, a.CompareTo(b));
        }

        [TestMethod()]
        public void Test_Greater_Then()
        {
            a = new CurrencyMatch(1, "100.00", null, false);
            b = new CurrencyMatch(2, "50.00", null, false);
            Assert.AreEqual(1, a.CompareTo(b));

            a = new CurrencyMatch(1, "50.00", "Total", false);
            b = new CurrencyMatch(2, "50.00", null, false);
            Assert.AreEqual(1, a.CompareTo(b));

            a = new CurrencyMatch(1, "50.00", null, true);
            b = new CurrencyMatch(2, "50.00", null, false);
            Assert.AreEqual(1, a.CompareTo(b));

            a = new CurrencyMatch(1, "50.00", "Total", true);
            b = new CurrencyMatch(2, "50.00", null, true);
            Assert.AreEqual(1, a.CompareTo(b));
        }
    }
}
