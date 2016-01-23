
namespace Common.Utils
{
    /// <summary>
    /// Singleton base class that is thread-safe.
    /// </summary>
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        /// <summary>
        /// The static reference.
        /// </summary>
        private static T _instance;

        /// <summary>
        /// Access to the singleton.
        /// </summary>
        public static T Instance
        {
            get { return _instance ?? (_instance = new T()); }
        }
    }
}
