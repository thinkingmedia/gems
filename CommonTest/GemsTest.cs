using System;
using Common.Utils;

namespace CommonTest
{
    /// <summary>
    /// Base class that can be used by other testing projects.
    /// </summary>
    public abstract class GemsTest
    {
        /// <summary>
        /// The resource reader.
        /// </summary>
        protected readonly Resources Reader;

        /// <summary>
        /// Constructor
        /// </summary>
        protected GemsTest()
        {
            Reader = new Resources(GetType(), "Data");
        }

        /// <summary>
        /// Loads a resource from the application as a string.
        /// </summary>
        [Obsolete("Use this.Reader.ReadAsString() instead")]
        protected string getResourceAsString(string pResourceName)
        {
            return Reader.ReadAsString(pResourceName, true, true);
        }
    }
}