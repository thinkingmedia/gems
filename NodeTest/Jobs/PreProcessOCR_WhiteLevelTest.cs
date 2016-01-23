using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Jobs;

namespace NodeTest.Jobs
{
    /// <summary>
    ///This is a test class for PreProcessOCR_WhiteLevelTest and is intended
    ///to contain all PreProcessOCR_WhiteLevelTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PreProcessOCR_WhiteLevelTest
    {
        private string path = @"C:\work\c-sharp\svn-faparks\e3m-node\NodeTest\Files";

        /// <summary>
        ///A test for WhiteLevel Constructor
        ///</summary>
        [TestMethod()]
        public void PreProcessOCR_WhiteLevelConstructorTest()
        {
            using (PreProcessOCR.WhiteLevel wl = new PreProcessOCR.WhiteLevel(path, "Blank-Front-24bit-Cropped.TIF", PreProcessOCR.WhiteLevel.DEFAULT_WHITE_LEVEL, PreProcessOCR.WhiteLevel.DEFAULT_WHITE_PERCENTAGE))
            {
                Assert.IsTrue(wl.IsBlank(), "Is not blank!");
            }

            using (PreProcessOCR.WhiteLevel wl = new PreProcessOCR.WhiteLevel(path, "Blank-Back-24bit-Cropped.TIF", PreProcessOCR.WhiteLevel.DEFAULT_WHITE_LEVEL, PreProcessOCR.WhiteLevel.DEFAULT_WHITE_PERCENTAGE))
            {
                Assert.IsTrue(wl.IsBlank(), "Is not blank!");
            }

            using (PreProcessOCR.WhiteLevel wl = new PreProcessOCR.WhiteLevel(path, "Blank-Front-24bit-Marked.TIF", PreProcessOCR.WhiteLevel.DEFAULT_WHITE_LEVEL, PreProcessOCR.WhiteLevel.DEFAULT_WHITE_PERCENTAGE))
            {
                Assert.IsTrue(wl.IsBlank(), "Is not blank!");
            }

            using (PreProcessOCR.WhiteLevel wl = new PreProcessOCR.WhiteLevel(path, "Blank-Back-24bit-Marked.TIF", PreProcessOCR.WhiteLevel.DEFAULT_WHITE_LEVEL, PreProcessOCR.WhiteLevel.DEFAULT_WHITE_PERCENTAGE))
            {
                Assert.IsTrue(wl.IsBlank(), "Is not blank!");
            }

            using (PreProcessOCR.WhiteLevel wl = new PreProcessOCR.WhiteLevel(path, "Travel-Document.TIF", PreProcessOCR.WhiteLevel.DEFAULT_WHITE_LEVEL, PreProcessOCR.WhiteLevel.DEFAULT_WHITE_PERCENTAGE))
            {
                Assert.IsFalse(wl.IsBlank(), "Should not be blank!");
            }
        }
    }
}
