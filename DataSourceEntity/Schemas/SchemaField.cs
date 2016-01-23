using System;
using System.Collections.Generic;
using Common.Events;

namespace DataSourceEntity.Schemas
{
    /// <summary>
    /// Describes a schema
    /// </summary>
    public class SchemaField
    {
        /// <summary>
        /// The order for the fields.
        /// </summary>
        public readonly int Order;

        /// <summary>
        /// The comment for the field.
        /// </summary>
        private readonly string _comment;

        /// <summary>
        /// The default value
        /// </summary>
        private readonly string _default;

        /// <summary>
        /// Is the field indexed.
        /// </summary>
        private readonly bool _indexed;

        /// <summary>
        /// Fieldname
        /// </summary>
        private readonly string _name;

        /// <summary>
        /// Can field be null.
        /// </summary>
        private readonly bool _nullable;

        /// <summary>
        /// Field type
        /// </summary>
        private readonly string _type;

        /// <summary>
        /// Is this field unsigned.
        /// </summary>
        private readonly bool _isUnsigned;

        /// <summary>
        /// Returns the method name for Record
        /// </summary>
        private string getFunc()
        {
            switch (_type)
            {
                case "char":
                case "varchar":
                case "text":
                case "longtext":
                case "shorttext":
                    return "getString";

                case "decimal":
                    return "getDecimal";
                case "double":
                    return "getDouble";
                case "float":
                    return "getFloat";

                case "date":
                case "datetime":
                    return "getDateTime";

                case "enum":
                    return "getUInt32";

                case "int":
                    return _isUnsigned ? "getUInt32" : "getInt32";

                case "bigint":
                    return _isUnsigned ? "getUInt64" : "getInt64";

                case "binary":
                case "varbinary":
                case "longblob":
                case "blob":
                    return "getBlob";

                case "tinyint":
                    return "getBoolean";
            }
            return "getString";
        }

        /// <summary>
        /// Gets the fieldname as a property name.
        /// </summary>
        private string getProperty()
        {
            if (_name.EndsWith("_id"))
            {
                return Inflector.Camelize(_name.Substring(0, _name.Length - 3)) + "ID";
            }
            return _name == "id" ? "ID" : Inflector.Camelize(_name);
        }

        /// <summary>
        /// Returns the C# variable type.
        /// </summary>
        private string getType()
        {
            switch (_type)
            {
                case "char":
                case "varchar":
                case "text":
                case "longtext":
                case "shorttext":
                    return "string";

                case "decimal":
                    return _nullable ? "decimal?" : "decimal";
                case "double":
                    return _nullable ? "double?" : "double";
                case "float":
                    return _nullable ? "float?" : "float";

                case "date":
                case "datetime":
                    return _nullable ? "DateTime?" : "DateTime";

                case "enum":
                    return _nullable ? "uint?" : "uint";

                case "int":
                    {
                        if (_isUnsigned)
                        {
                            return _nullable ? "uint?" : "uint";
                        }
                        return _nullable ? "int?" : "int";
                    }

                case "bigint":
                {
                    if (_isUnsigned)
                    {
                        return _nullable ? "ulong?" : "ulong";
                    }
                    return _nullable ? "long?" : "long";
                }

                case "binary":
                case "varbinary":
                case "longblob":
                case "blob":
                    return "byte[]";

                case "tinyint":
                    return _nullable ? "bool?" : "bool";
            }
            return "string";
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SchemaField(String pName,
                           string pType,
                           string pColumnType,
                           bool pNullable,
                           string pComment,
                           bool pIndexed,
                           int pOrder,
                           string pDefault)
        {
            _name = pName;

            _type = pType;
            _isUnsigned = pColumnType.Trim().ToLower().EndsWith("unsigned");
            _nullable = pNullable;
            _comment = pComment;
            _indexed = pIndexed;
            Order = pOrder;
            _default = pDefault;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="pOther"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="pOther">An object to compare with this object.</param>
        public override bool Equals(object pOther)
        {
            SchemaField other = pOther as SchemaField;
            return other != null && _name == other._name && _type == other._type && _nullable == other._nullable;
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        public override int GetHashCode()
        {
            return _name.GetHashCode() ^ _type.GetHashCode() ^ _nullable.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0}{1}:{2}", _type, _nullable ? "?" : "", _name);
        }

        /// <summary>
        /// Creates a dictionary list of property values for use in the templates.
        /// </summary>
        public Dictionary<string, object> getTemplateArguments()
        {
            return new Dictionary<string, object>
                   {
                       {"Property", getProperty()},
                       {"FieldName", _name},
                       {"FieldIdentifier", _name.ToUpper()},
                       {"Type", getType()},
                       {"Func", getFunc()},
                       {"Nullable", _nullable},
                       {"Comment", _comment},
                       {"Order",Order},
                       {"Indexed",_indexed},
                       {"Default",_default}
                   };
        }
    }
}