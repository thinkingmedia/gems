
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestFramework
{
    /// <summary>
    /// Assertion checks that work on collections of values passed as arguments to a function.
    /// </summary>
    public static class All
    {
        /// <summary>
        /// Tests that all the returns from the function with the given
        /// argument are true.
        /// </summary>
        /// <typeparam name="TArg">First argument type.</typeparam>
        /// <param name="pFunc">The function reference.</param>
        /// <param name="pValues">Array of test values.</param>
        public static void areTrue<TArg>(Func<TArg, bool> pFunc, params TArg[] pValues)
        {
            foreach (TArg value in pValues)
            {
                Assert.IsTrue(pFunc(value), value.ToString());
            }
        }

        /// <summary>
        /// Same as allTrue except tested for False.
        /// </summary>
        /// <typeparam name="TArg">First argument type.</typeparam>
        /// <param name="pFunc">The function reference.</param>
        /// <param name="pValues">Array of test values.</param>
        public static void areFalse<TArg>(Func<TArg, bool> pFunc, params TArg[] pValues)
        {
            foreach (TArg value in pValues)
            {
                Assert.IsFalse(pFunc(value), value.ToString());
            }
        }

        /// <summary>
        /// Tests that all the values generate the same return value.
        /// </summary>
        /// <typeparam name="TArg">First argument type.</typeparam>
        /// <param name="pFunc">The function reference.</param>
        /// <param name="pExpected">Expected return value.</param>
        /// <param name="pValues">Array of test values.</param>
        public static void areEqual<TArg>(Func<TArg, bool> pFunc, TArg pExpected, params TArg[] pValues)
        {
            foreach (TArg value in pValues)
            {
                Assert.AreEqual(pExpected, pFunc(value), value.ToString());
            }
        }

        /// <summary>
        /// Tests that all the values in a dictionary represent the input and output
        /// of a function. Key is the return value and Value is the argument.
        /// </summary>
        /// <typeparam name="TReturn">Return type</typeparam>
        /// <typeparam name="TArg">Argument type</typeparam>
        /// <param name="pFunc">Function reference</param>
        /// <param name="pValues">Test data</param>
        public static void areEqual<TReturn, TArg>(Func<TArg, TReturn> pFunc, Dictionary<TReturn, TArg> pValues)
        {
            foreach (KeyValuePair<TReturn, TArg> pair in pValues)
            {
                Assert.AreEqual(pair.Key, pFunc(pair.Value));
            }
        }

        /// <summary>
        /// Tests that all the values cause the function to throw the expected exception type.
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="pException"></param>
        /// <param name="pFunc"></param>
        /// <param name="pValues"></param>
        public static void raiseException<TReturn, TArg>(Type pException, Func<TArg, TReturn> pFunc, params TArg[] pValues)
        {
            foreach (TArg value in pValues)
            {
                try
                {
                    pFunc(value);
                    Assert.Fail("Exception {0} was expected.", pException.FullName);
                }
                catch (Exception exception)
                {
                    if (exception.GetType() != pException)
                    {
                        Assert.Fail("Exception {0} is wrong type.", exception.GetType().FullName);
                    }
                }
            }
        }

        /// <summary>
        /// Checks that the function returns the same value for all the arguments.
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="pFunc"></param>
        /// <param name="pReturn"></param>
        /// <param name="pValues"></param>
        public static void returnSame<TReturn, TArg>(Func<TArg, TReturn> pFunc, TReturn pReturn, params TArg[] pValues)
        {
            foreach (TArg value in pValues)
            {
                TReturn result = pFunc(value);
                Assert.AreEqual(pReturn, result, value.ToString());
            }
        }
    }
}
