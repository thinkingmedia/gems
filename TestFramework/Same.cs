
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestFramework
{
    /// <summary>
    /// Assertion checks for checking if two collections are the same.
    /// </summary>
    public static class Same
    {
        /// <summary>
        /// Checks if two dictionaries contain the same values. Supports recursive
        /// dictionaries and collections as values.
        /// </summary>
        /// <param name="pExpect">Expected value</param>
        /// <param name="pActual">Actual value</param>
        // ReSharper disable CanBeReplacedWithTryCastAndCheckForNull
        public static void dictionary(IDictionary pExpect, IDictionary pActual)
        {
            Assert.IsNotNull(pExpect);
            Assert.IsNotNull(pActual);

            if (pExpect.Keys.Count != pActual.Keys.Count)
            {
                Assert.Fail("Expected {0} keys, but contains {1} keys.", pExpect.Keys.Count, pActual.Keys.Count);
            }

            object[] expectKeys = new object[pExpect.Keys.Count];
            object[] actualKeys = new object[pActual.Keys.Count];

            pExpect.Keys.CopyTo(expectKeys, 0);
            pActual.Keys.CopyTo(actualKeys, 0);

            // check if the two key sets are the same
            CollectionAssert.AreEquivalent(expectKeys, actualKeys);

            for (int i = 0, c = expectKeys.Length; i < c; i++)
            {
                object expect = pExpect[expectKeys[i]];
                object actual = pActual[expectKeys[i]];

                // both can be null
                if (expect == null && actual == null)
                {
                    continue;
                }

                // both must be assigned a value
                Assert.IsNotNull(expect);
                Assert.IsNotNull(actual);

                // must be same types
                Assert.AreEqual(expect.GetType(), actual.GetType());

                if (expect is IDictionary)
                {
                    // support recursive dictionary checks
                    dictionary((IDictionary)expect, (IDictionary)actual);
                }
                else if (expect is ICollection)
                {
                    CollectionAssert.AreEquivalent((ICollection)expect, (ICollection)actual);
                }
                else
                {
                    Assert.AreEqual(expect, actual);
                }
            }
        }
    }
}
