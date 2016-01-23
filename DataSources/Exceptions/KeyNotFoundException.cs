
namespace DataSources.Exceptions
{
    public class KeyNotFoundException : BehaviorException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public KeyNotFoundException(string pModel, string pKey)
            : base(@"Record {0}::{1} not found.", pModel, pKey)
        {
        }
    }
}
