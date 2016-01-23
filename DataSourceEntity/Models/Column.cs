using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataSourceEntity.BaseClass;
using DataSourceEntity.Schemas;
using DataSources;
using DataSources.DataSource;
using DataSources.Query;

namespace DataSourceEntity.Models
{
    /// <summary>
    /// Used to access MySQL internal table for
    /// describing the columns in a table.
    /// </summary>
    public class Column : Model
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Column(iDataSource pSource)
            : base(new ModelOptions { Table = "COLUMNS" }, pSource)
        {
        }

        /// <summary>
        /// Gets the schema for a table.
        /// </summary>
        public SchemaTable getSchema(string pDatabase, string pTableName)
        {
            Records records = Select()
                .Where()
                .Eq("TABLE_SCHEMA", pDatabase)
                .Eq("TABLE_NAME", pTableName)
                .End()
                .All();

            List<SchemaField> fields = (from record in records
                                        select new SchemaField(
                                            record.getString("COLUMN_NAME"),
                                            record.getString("DATA_TYPE"),
                                            record.getString("COLUMN_TYPE"),
                                            record.getString("IS_NULLABLE") == "YES",
                                            record.getString("COLUMN_COMMENT"),
                                            !record.isNullOrEmpty("COLUMN_KEY"),
                                            record.getInt32("ORDINAL_POSITION"),
                                            record.getString("COLUMN_DEFAULT")
                                            ))
                                        .OrderBy(pEntry => pEntry.Order)
                                        .ToList();

            return new SchemaTable(pTableName, fields, false);
        }
    }
}
