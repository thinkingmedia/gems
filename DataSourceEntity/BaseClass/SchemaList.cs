using System.Collections.Generic;
using System.Linq;
using DataSourceEntity.Exceptions;
using DataSourceEntity.Schemas;

namespace DataSourceEntity.BaseClass
{
    /// <summary>
    /// Holds a list of tables that will be merged.
    /// </summary>
    public class SchemaList
    {
        /// <summary>
        /// A list of description.
        /// </summary>
        private readonly List<MergeDesc> _desciptions;

        /// <summary>
        /// The schema for the abstract classes.
        /// </summary>
        public readonly List<SchemaTable> Abstracts;

        /// <summary>
        /// The schemas for the database.
        /// </summary>
        public readonly List<SchemaTable> Schemas;

        /// <summary>
        /// Constructor
        /// </summary>
        public SchemaList(string pExpression, List<SchemaTable> pSchemas)
        {
            Abstracts = new List<SchemaTable>();

            _desciptions = new List<MergeDesc>();
            if (!string.IsNullOrWhiteSpace(pExpression))
            {
                ParseExpression(pExpression);
            }

            Schemas = _desciptions.Aggregate(pSchemas, Merge);
        }

        /// <summary>
        /// Parses the murge expression into a list of MergeDesc
        /// </summary>
        private void ParseExpression(string pExpression)
        {
            string[] expressions = pExpression.Split(new[] { ',' });
            foreach (string expression in expressions)
            {
                string[] parts = expression.Split(new[] { '=' });
                if (parts.Length != 2)
                {
                    throw new MergeException("Invalid merge expression. Must be table+table=class");
                }
                string[] tables = parts[0].Split(new[] { '+' });
                string className = parts[1];
                if (tables.Length <= 1)
                {
                    throw new MergeException("Must merge 2 or more tables.");
                }
                _desciptions.Add(new MergeDesc(className, tables));
            }
        }

        /// <summary>
        /// Merges the schema for a list of tables.
        /// </summary>
        private List<SchemaTable> Merge(List<SchemaTable> pSchema, MergeDesc pDesc)
        {
            Dictionary<SchemaField, int> count = new Dictionary<SchemaField, int>();
            foreach (SchemaField entry in from x in pSchema where pDesc.Tables.Contains(x.Table) from y in x.Fields select y)
            {
                if (count.ContainsKey(entry))
                {
                    count[entry]++;
                    continue;
                }
                count.Add(entry, 1);
            }

            // these will be the fields for the base class
            List<SchemaField> common = (from c in count where c.Value >= 2 select c.Key).ToList();
            Abstracts.Add(new SchemaTable(pDesc.ClassName, common, true));

            // remove these fields from the parent tables
            foreach (string table in pDesc.Tables)
            {
                SchemaTable first = (from x in pSchema where x.Table == table select x).FirstOrDefault();
                if (first == null)
                {
                    throw new MergeException("Table {0} for merge not found.", table);
                }

                first.Fields = (
                    from entry in first.Fields
                    where !common.Contains(entry)
                    select entry)
                    .ToList();

                first.BaseClass = pDesc.ClassName;
            }

            return pSchema;
        }
    }
}