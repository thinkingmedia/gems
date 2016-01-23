using System.Collections.Generic;
using System.Linq;

namespace DataSources.Query
{
    public class Fields : AbstractQueryFeature, iFields
    {
        /// <summary>
        /// The field values
        /// </summary>
        public readonly List<FieldValue> Values;

        /// <summary>
        /// The number of fields.
        /// </summary>
        public int Count
        {
            get { return Values.Count; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Fields(iQueryBuilder pQuery)
            : base(pQuery)
        {
            Values = new List<FieldValue>();
        }

        /// <summary>
        /// Adds a field value to the current field collection.
        /// </summary>
        public iQueryBuilder Add(FieldValue pFieldValue)
        {
            Values.Add(pFieldValue);
            return Query();
        }

        /// <summary>
        /// Adds a field value to the current field collection.
        /// </summary>
        public iQueryBuilder Add(IEnumerable<FieldValue> pFieldValue)
        {
            Values.AddRange(pFieldValue);
            return Query();
        }

        /// <summary>
        /// Adds a field value to the current field collection.
        /// </summary>
        public iQueryBuilder Add(string pFieldname, string pValue)
        {
            Values.Add(new FieldValue(pFieldname, pValue));
            return Query();
        }

        /// <summary>
        /// Gets the first field value for the fieldname or Null.
        /// </summary>
        public FieldValue Get(string pFieldname)
        {
            return Values.FirstOrDefault(pField=>pField.Name == pFieldname);
        }

        /// <summary>
        /// Clears the fields collection.
        /// </summary>
        public iQueryBuilder Clear()
        {
            Values.Clear();
            return Query();
        }

        /// <summary>
        /// Does the query contain a field.
        /// </summary>
        public bool HasField(string pFieldname)
        {
            return Values.Any(pField=>pField.Name == pFieldname);
        }

        /// <summary>
        /// Ends editing the fields.
        /// </summary>
        public iQueryBuilder End()
        {
            return Query();
        }
    }
}