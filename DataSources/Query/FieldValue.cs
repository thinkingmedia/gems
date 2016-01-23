namespace DataSources.Query
{
    public class FieldValue
    {
        /// <summary>
        /// The name of the column
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// The value to assign to the column. This
        /// is valid only for UPDATE and INSERT
        /// </summary>
        public readonly string Value;

        /// <summary>
        /// Constructor
        /// </summary>
        public FieldValue(string pName)
        {
            Name = pName;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FieldValue(string pName, string pValue)
            : this(pName)
        {
            Value = pValue;
        }

        /// <summary>
        /// Converts a string to a field.
        /// </summary>
        public static implicit operator FieldValue(string pName)
        {
            return new FieldValue(pName);
        }

        /// <summary>
        /// Converts a field to a string (column name).
        /// </summary>
        public static implicit operator string(FieldValue pFieldValue)
        {
            return pFieldValue.Name;
        }
    }
}