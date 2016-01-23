using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Scan.Bayesian;

namespace NodeTest.Scan.Bayesian
{
    /// <summary>
    ///This is a test class for ProbabilityTest and is intended
    ///to contain all ProbabilityTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProbabilityTest
    {
        /// <summary>
        ///A test for Probability Constructor
        ///</summary>
        [TestMethod()]
        public void Test_NaN()
        {
            Probability p = new Probability();
            p.Add("one", 1.0);
            p.Add("two", 2.0);
            p.Add("three", double.NaN);

            Assert.AreEqual(2, p.Prob.Count);
        }

        [TestMethod()]
        public void Test_Serializing()
        {
            Probability p = new Probability();
            p.Add("one", 1.0);
            p.Add("two", 2.0);
            p.Add("three", 3.0);
            Assert.AreEqual(3, p.Prob.Count);

            string str = p.ToString();
            p = new Probability(str);
            Assert.AreEqual(3, p.Prob.Count);
            Assert.IsTrue(p.Prob.ContainsKey("one"));
            Assert.IsTrue(p.Prob.ContainsKey("two"));
            Assert.IsTrue(p.Prob.ContainsKey("three"));
            Assert.IsTrue(p.Prob.ContainsValue(1.0));
            Assert.IsTrue(p.Prob.ContainsValue(2.0));
            Assert.IsTrue(p.Prob.ContainsValue(3.0));
        }
    }
}
