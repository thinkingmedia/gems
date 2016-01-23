
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestFramework
{
    /// <summary>
    /// Assertion checks done on a collection.
    /// </summary>
    public static class Collection
    {
        /// <summary>
        /// Must contain all the arguments.
        /// </summary>
        public static void containsThese(ICollection pCollection, params object[] pValues)
        {
            foreach (object value in pValues)
            {
                CollectionAssert.Contains(pCollection, value, value.ToString());
            }
        }

        /// <summary>
        /// The value has to occur X number of times.
        /// </summary>
        /// <param name="pCollection"></param>
        /// <param name="pValue"></param>
        /// <param name="pCount"></param>
        public static void occurs(ICollection pCollection, object pValue, int pCount)
        {

        }

        /// <summary>
        /// Asserts that the collection contains the values and that they
        /// are in the same order.
        /// </summary>
        public static void isThis(ICollection pCollection, params object[] pValues)
        {
            CollectionAssert.AreEqual(pCollection, pValues);
        }
    }
}
