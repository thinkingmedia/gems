using System;
using System.Collections.Generic;
using System.Linq;
using DataSources.Exceptions;
using DataSources.Query;

namespace DataSources.DataSource
{
    /// <summary>
    /// Handles the rendering of a SQL string from data.
    /// </summary>
    internal static class MySqlRender
    {
        /// <summary>
        /// Comma separator for query sets.
        /// </summary>
        private const string _COMMA = ",";

        /// <summary>
        /// Formats a fieldname.
        /// </summary>
        private static string Field(string pTable, string pField)
        {
            // does field contain an expression
            if (pField.Contains('(') && pField.Contains(')'))
            {
                return pField;
            }
            // other types of expressions
            if (pField.ToUpper().Contains(" AS ") || pField.ToUpper().Contains(" WITH "))
            {
                return pField;
            }

            return string.Format("`{0}`.`{1}`", pTable, pField);
        }

        /// <summary>
        /// MySQL date format
        /// </summary>
        public static string Date(DateTime pDate)
        {
            return pDate.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// List of fields
        /// </summary>
        public static string Fields(string pTable, Fields pFields)
        {
            return pFields.Count == 0
                ? string.Format("`{0}`.*", pTable)
                : string.Join(_COMMA, from field in pFields.Values
                                      select Field(pTable, field.Name));
        }

        /// <summary>
        /// List of fields for group by
        /// </summary>
        public static string Groups(string pTable, iFields pFields)
        {
            Fields fields = (Fields)pFields;
            return string.Join(_COMMA, from field in fields.Values
                                       select Field(pTable, field.Name));
        }

        /// <summary>
        /// Renders the JOIN for a query.
        /// </summary>
        public static string Join(Joinable pJoin)
        {
            if (pJoin.Type == eJOIN.NONE)
            {
                throw new ModelException("JOIN type not set for query.");
            }

            string type = pJoin.Type.ToString().Replace("_", " ");
            string on = ((Conditions)pJoin.Where).Root.Render();

            IEnumerable<string> tables = pJoin.JoinTables.Select(pJoinTable => string.Format("`{0}`",pJoinTable.Table));

            return string.Format("{0} JOIN ({1}) ON {2}", type, string.Join(_COMMA, tables), on);
        }

        /// <summary>
        /// Order clause
        /// </summary>
        public static string Order(QueryBuilder pQuery)
        {
            return string.Join(_COMMA,
                from orderBy in pQuery.Order select string.Format("{0} {1}", orderBy.Value, orderBy.Key));
        }

        /// <summary>
        /// Field assignments
        /// </summary>
        public static string Set(iFields pFields)
        {
            Fields fields = (Fields)pFields;

            return string.Join(_COMMA, from field in fields.Values
                                       select string.Format("`{0}`={1}", field.Name, field.Value));
        }

        /// <summary>
        /// Creates a SET statement for creating local variables.
        /// </summary>
        public static string Variables(iParameters pVariables)
        {
            Parameters vars = (Parameters)pVariables;
            if (vars.Count == 0)
            {
                return "";
            }
            IEnumerable<string> values = from pair in vars.Data
                                         select string.Format("{0}={1}", pair.Key, pair.Value);

            return string.Format("SET {0};", string.Join(_COMMA, values));
        }
    }
}