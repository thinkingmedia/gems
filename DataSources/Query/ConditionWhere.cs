using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSources.Query
{
    public static class ConditionWhere
    {
        /// <summary>
        /// Where a field is equal to a value.
        /// </summary>
        public static iCondition Eq(this iCondition pCon, string pFieldName, object pValue)
        {
            return Op(pCon, pFieldName, "=", pValue);
        }

        /// <summary>
        /// Where a field is equal to a value, or is null when value is also null.
        /// </summary>
        public static iCondition EqOrNull(this iCondition pCon, string pFieldName, object pValue)
        {
            return pValue == null 
                ? isNull(pCon, pFieldName) 
                : Eq(pCon, pFieldName, pValue);
        }

        /// <summary>
        /// Adds the expression to the query conditions.
        /// </summary>
        public static iCondition Expression(this iCondition pCon, string pExpression)
        {
            Conditions con = (Conditions)pCon;
            con.Current.Add(pExpression);
            return con;
        }

        /// <summary>
        /// Adds the expression to the query conditions for a given fieldname.
        /// </summary>
        public static iCondition Expression(this iCondition pCon, string pFieldName, string pExpression)
        {
            string str = string.Format("{0} {1}", pCon.Field(pFieldName), pExpression);
            ((Conditions)pCon).Current.Add(str);
            return pCon;
        }

        /// <summary>
        /// Where a field value is one in a set.
        /// </summary>
        public static iCondition In(this iCondition pCon, string pFieldName, ICollection<UInt32> pValues)
        {
            return pValues.Count == 0
                ? pCon
                : Expression(pCon, pFieldName, string.Format("IN ({0})", string.Join(",", pValues)));
        }

        /// <summary>
        /// Where a field value is one in a set.
        /// </summary>
        public static iCondition In(this iCondition pCon, string pFieldName, ICollection<string> pValues)
        {
            IEnumerable<string> strings = from str in pValues select string.Format("'{0}'", str);
            return pValues.Count == 0
                ? pCon
                : Expression(pCon, pFieldName, string.Format("IN ({0})", string.Join(",", strings)));
        }

        /// <summary>
        /// Where a field is NOT equal to a value.
        /// </summary>
        public static iCondition NotEq(this iCondition pCon, string pFieldName, object pValue)
        {
            return Op(pCon, pFieldName, "!=", pValue);
        }

        /// <summary>
        /// Creates a where statement with an operator.
        /// </summary>
        public static iCondition Op(this iCondition pCon, string pFieldName, string pOperator, object pValue)
        {
            Conditions con = (Conditions)pCon;
            string name = con.getNextParameterName();
            con.Param(name, pValue is Guid ? ((Guid)pValue).ToByteArray() : pValue);
            string str = string.Format("{0} {1} @{2}", con.Field(pFieldName), pOperator, name);
            con.Current.Add(str);
            return con;
        }

        /// <summary>
        /// Where a field is value is equal to 0.
        /// </summary>
        public static iCondition isFalse(this iCondition pCon, string pFieldName)
        {
            return Op(pCon, pFieldName, "=", 0);
        }

        /// <summary>
        /// Matches a record by it's GUID.
        /// </summary>
        public static iCondition isGuid(this iCondition pCon, Guid pGuid)
        {
            return Eq(pCon, pCon.PrimaryKey(), pGuid);
        }

        /// <summary>
        /// Matches a record by it's ID.
        /// </summary>
        public static iCondition isID(this iCondition pCon, UInt32 pID)
        {
            return Eq(pCon, pCon.PrimaryKey(), pID);
        }

        /// <summary>
        /// Matches a record by it's ID.
        /// </summary>
        public static iCondition isID(this iCondition pCon, PrimaryValue pID)
        {
            return Eq(pCon, pCon.PrimaryKey(), pID);
        }

        /// <summary>
        /// Matches a record that is NOT the same ID.
        /// </summary>
        public static iCondition isNotID(this iCondition pCon, UInt32 pID)
        {
            return NotEq(pCon, pCon.PrimaryKey(), pID);
        }

        /// <summary>
        /// Where the field is not null.
        /// </summary>
        public static iCondition isNotNull(this iCondition pCon, string pFieldName)
        {
            Conditions con = (Conditions)pCon;
            con.Current.Add(string.Format("{0} IS NOT NULL", pCon.Field(pFieldName)));
            return pCon;
        }

        /// <summary>
        /// Where the field is null.
        /// </summary>
        public static iCondition isNull(this iCondition pCon, string pFieldName)
        {
            Conditions con = (Conditions)pCon;
            con.Current.Add(string.Format("{0} IS NULL", pCon.Field(pFieldName)));
            return pCon;
        }

        /// <summary>
        /// Where a field is value is equal to 1.
        /// </summary>
        public static iCondition isTrue(this iCondition pCon, string pFieldName)
        {
            return Op(pCon, pFieldName, "=", 1);
        }
    }
}