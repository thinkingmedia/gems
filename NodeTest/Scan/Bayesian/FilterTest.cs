using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Scan.Bayesian;

namespace NodeTest.Scan.Bayesian
{
    /// <summary>
    ///This is a test class for FilterTest and is intended
    ///to contain all FilterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FilterTest
    {
        /// <summary>
        ///A test for Filter Constructor
        ///</summary>
        [TestMethod()]
        public void Test_Matching()
        {
            Corpus good = new Corpus();
            Corpus bad = new Corpus();

            good.Add("the chicken jumped over the moon", 3);
            bad.Add("the cow ran threw the moon", 3);

            Calculator c = new Calculator(Calculator.Defaults);
            Probability prob = c.CalculateProbabilities(good, bad);

            Filter target = new Filter(prob);

            target.Test("the cow ran over the moon", 3);

            Assert.IsTrue(target.Test("the cow ran threw the moon", 3) > 0.98);
            Assert.IsTrue(target.Test("the cow ran over the moon", 3) > 0.25);
        }
    }
}
