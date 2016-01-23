using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Scan.Bayesian;

namespace NodeTest.Scan.Bayesian
{
    /// <summary>
    ///This is a test class for CalculatorTest and is intended
    ///to contain all CalculatorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CalculatorTest
    {
        [TestMethod()]
        public void Test_Token_Probability()
        {
            Corpus good = new Corpus();
            Corpus bad = new Corpus();

            good.Add("the chicken jumped over the moon", 3);
            bad.Add("the cow ran threw the moon", 3);

            Calculator c = new Calculator(Calculator.Defaults);

            Assert.AreEqual<double>(0.3333333333333333, c.CalculateTokenProbability("the", good, bad));
            Assert.AreEqual<double>(0.3333333333333333, c.CalculateTokenProbability("moon", good, bad));
            //Assert.AreEqual<double>(Calculator.Defaults.LikelySpamScore, c.CalculateTokenProbability("ran", good, bad));
            //Assert.AreEqual<double>(Calculator.Defaults.LikelySpamScore, c.CalculateTokenProbability("cow", good, bad));
        }

        [TestMethod()]
        public void Test_Calculate_Probability()
        {
            Corpus good = new Corpus();
            Corpus bad = new Corpus();

            good.Add("the chicken jumped over the moon", 3);
            bad.Add("the cow ran threw the moon", 3);

            Calculator c = new Calculator(Calculator.Defaults);
            Probability prob = c.CalculateProbabilities(good, bad);

            Assert.AreEqual<double>(0.3333333333333333, prob.Prob["the"]);
            Assert.AreEqual<double>(0.3333333333333333, prob.Prob["moon"]);
            //Assert.AreEqual<double>(Calculator.Defaults.LikelySpamScore, prob.Prob["ran"]);
            //Assert.AreEqual<double>(Calculator.Defaults.LikelySpamScore, prob.Prob["cow"]);
        }
    }
}
