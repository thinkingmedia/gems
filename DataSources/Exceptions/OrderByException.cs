
namespace DataSources.Exceptions
{
    public class OrderByException : ModelException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderByException(string pMessage, params object[] pValues)
            : base(pMessage, pValues)
        {
        }
    }
}
