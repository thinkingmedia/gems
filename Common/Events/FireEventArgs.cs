using System;

namespace Common.Events
{
    /// <summary>
    /// Generalized event arguments class.
    /// </summary>
    public class FireEventArgs<TArgType> : EventArgs
    {
        /// <summary>
        /// The value as a property.
        /// </summary>
        public TArgType Value { get; private set; }

        /// <summary>
        /// The constructor.
        /// </summary>
        public FireEventArgs(TArgType pValue)
        {
            Value = pValue;
        }
    }
}
