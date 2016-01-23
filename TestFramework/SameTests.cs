using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestFramework
{
    [TestClass]
    public class SameTests
    {
        [TestMethod]
        public void dictionary()
        {
            Same.dictionary(
                new Dictionary<int, int> { { 0, 0 }, { 1, 1 } },
                new Dictionary<int, int> { { 1, 1 }, { 0, 0 } }
                );
        }
    }
}
