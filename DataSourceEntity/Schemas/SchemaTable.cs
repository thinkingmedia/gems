using System.Collections.Generic;
using Common.Events;

namespace DataSourceEntity.Schemas
{
    /// <summary>
    /// Describes a table
    /// </summary>
    public class SchemaTable
    {
        /// <summary>
        /// Is this a base class for other tables?
        /// </summary>
        public readonly bool IsAbstract;

        /// <summary>
        /// The name of the table.
        /// </summary>
        public readonly string Table;

        /// <summary>
        /// The name of the base class to use in the template.
        /// </summary>
        public string BaseClass;

        /// <summary>
        /// The fields for the table.
        /// </summary>
        public List<SchemaField> Fields;

        /// <summary>
        /// Converts a table name into a C# object name.
        /// </summary>
        private static string getClassified(string pName)
        {
            return Inflector.Classify(pName) + "Entity";
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SchemaTable(string pTable, List<SchemaField> pFields, bool pIsAbstract)
        {
            Table = pTable;
            Fields = pFields;
            IsAbstract = pIsAbstract;
            BaseClass = "records";
        }

        /// <summary>
        /// The base class name.
        /// </summary>
        public string getBaseClass()
        {
            return getClassified(BaseClass);
        }

        /// <summary>
        /// The class name.
        /// </summary>
        public string getClassName()
        {
            return getClassified(Table);
        }

        /// <summary>
        /// The class name for fields.
        /// </summary>
        public string getFieldsName()
        {
            return Inflector.Classify(Table) + "Fields";
        }
    }
}