using System.Collections.Generic;

namespace DataSources.Query
{
    public interface iFields
    {
        /// <summary>
        /// Adds a field value to the current field collection.
        /// </summary>
        iQueryBuilder Add(FieldValue pFieldValue);

        /// <summary>
        /// Adds a field value to the current field collection.
        /// </summary>
        iQueryBuilder Add(IEnumerable<FieldValue> pFieldValue);

        /// <summary>
        /// Adds a field value to the current field collection.
        /// </summary>
        iQueryBuilder Add(string pFieldname, string pValue);

        /// <summary>
        /// Clears the fields collection.
        /// </summary>
        iQueryBuilder Clear();

        /// <summary>
        /// Ends editing the fields.
        /// </summary>
        iQueryBuilder End();

        /// <summary>
        /// Gets the first field value for the fieldname or Null.
        /// </summary>
        FieldValue Get(string pFieldname);

        /// <summary>
        /// Does the query contain a field.
        /// </summary>
        bool HasField(string pFieldname);
    }
}