using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Scan.Bayesian;

namespace NodeTest.Scan.Bayesian
{
    /// <summary>
    ///This is a test class for CorpusTest and is intended
    ///to contain all CorpusTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CorpusTest
    {
        /// <summary>
        ///</summary>
        [TestMethod()]
        public void Basic_Add()
        {
            Corpus c = new Corpus();
            c.Add("one");
            c.Add("two");
            c.Add("three");
            Assert.AreEqual(3, c.Tokens.Count);
        }

        [TestMethod()]
        public void Basic_List_Add()
        {
            Corpus c = new Corpus();
            c.Add(new string[] { "one", "two", "three" });
            Assert.AreEqual(3, c.Tokens.Count);
        }

        [TestMethod()]
        public void Basic_Builder_Add()
        {
            Corpus c = new Corpus();
            c.Add("one two three a 333 3adsf a123", 3);
            Assert.AreEqual(4, c.Tokens.Count);
        }
    }
}
