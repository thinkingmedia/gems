using System;

namespace TestFramework.Attributes
{
    /// <summary>
    /// Defines the runtime class type that the unit test is
    /// testing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AutoClass : AutoAttribute
    {
        /// <summary>
        /// The class to test.
        /// </summary>
        public Type type { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pType">The type to test.</param>
        public AutoClass(Type pType)
        {
            type = pType;
        }
    }
}
