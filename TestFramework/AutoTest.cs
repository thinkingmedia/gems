

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestFramework
{
    /// <summary>
    /// The base class used to assist in automating
    /// the testing of classes.
    /// </summary>
    // ReSharper disable UnusedMember.Global
    // ReSharper disable MemberCanBeProtected.Global
    [TestClass]
    public abstract class AutoTest
    {
        // ReSharper disable once InconsistentNaming
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        protected AutoTest()
        {

        }
    }
}
