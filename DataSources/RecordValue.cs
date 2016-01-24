using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DataSources.Query;
using GemsLogger;
using MySql.Data.Types;

namespace DataSources
{
    public class RecordValue : iRecord
    {
        /// <summary>
        /// Logging
        /// </summary>
        private static readonly Logger _logger = Logger.Create(typeof (RecordValue));

        /// <summary>
        /// The data for this record.
        /// </summary>
        private readonly Dictionary<string, object> _data;

        /// <summary>
        /// Returns the inner dictionary.
        /// </summary>
        public Dictionary<string, object> ToDictionary()
        {
            return _data;
        }

        /// <summary>
        /// The number of fields.
        /// </summary>
        public int Count
        {
            get { return _data.Count; }
        }

        /// <summary>
        /// Access the fields by fieldname.
        /// </summary>
        public object this[string pKey]
        {
            get { return _data.ContainsKey(pKey) ? _data[pKey] : null; }
            set { _data[pKey] = value; }
        }

        /// <summary>
        /// Access the fields by offset.
        /// </summary>
        public object this[int pIndx]
        {
            get { return _data.Count > pIndx ? _data.Values.ElementAt(pIndx) : null; }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RecordValue()
        {
            _data = new Dictionary<string, object>();
        }

        /// <summary>
        /// Reads a row from the database.
        /// </summary>
        public RecordValue(IDataRecord pReader)
            : this()
        {
            for (int i = 0; i < pReader.FieldCount; i++)
            {
                string name = pReader.GetName(i);
                try
                {
                    this[name] = pReader.IsDBNull(i) ? null : pReader.GetValue(i);
                }
                catch (MySqlConversionException ex)
                {
                    _logger.Exception(ex);
                    this[name] = null;
                }
            }
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public RecordValue(RecordValue pRecord)
            : this(pRecord._data)
        {
        }

        /// <summary>
        /// Dictionary Constructor
        /// </summary>
        public RecordValue(IDictionary<string, object> pData)
        {
            _data = new Dictionary<string, object>(pData);
        }

        /// <summary>
        /// Tells if the a fieldname exists.
        /// </summary>
        public bool Has(string pKey)
        {
            return _data.ContainsKey(pKey);
        }

        /// <summary>
        /// Removes a field.
        /// </summary>
        public void Remove(string pKey)
        {
            _data.Remove(pKey);
        }

        /// <summary>
        /// Assigns the values of this record to a query.
        /// </summary>
        public void Set(iQueryBuilder pQuery)
        {
            foreach (KeyValuePair<string, object> pair in _data)
            {
                pQuery.Set(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public byte[] getBlob(string pColumn)
        {
            return (byte[])this[pColumn];
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public bool getBoolean(string pColumn)
        {
            return Convert.ToBoolean(this[pColumn]);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public byte getByte(string pColumn)
        {
            return Convert.ToByte(this[pColumn]);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public char getChar(string pColumn)
        {
            return Convert.ToChar(this[pColumn]);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public DateTime getDateTime(string pColumn)
        {
            return Convert.ToDateTime(this[pColumn]);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public decimal getDecimal(string pColumn)
        {
            return Convert.ToDecimal(this[pColumn]);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public double getDouble(string pColumn)
        {
            return Convert.ToDouble(this[pColumn]);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public float getFloat(string pColumn)
        {
            return Convert.ToSingle(this[pColumn]);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public Guid getGuid(string pColumn)
        {
            return (Guid)this[pColumn];
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public short getInt16(string pColumn)
        {
            return Convert.ToInt16(this[pColumn]);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public int getInt32(string pColumn)
        {
            return Convert.ToInt32(this[pColumn]);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public long getInt64(string pColumn)
        {
            return Convert.ToInt64(this[pColumn]);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public string getString(string pColumn)
        {
            return Convert.ToString(this[pColumn]);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public ushort getUInt16(string pColumn)
        {
            return Convert.ToUInt16(this[pColumn]);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public UInt32 getUInt32(string pColumn)
        {
            return Convert.ToUInt32(this[pColumn]);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public ulong getUInt64(string pColumn)
        {
            return Convert.ToUInt64(this[pColumn]);
        }

        /// <summary>
        /// Tells if a column contains a NULL value.
        /// </summary>
        public bool isNull(string pColumn)
        {
            return Convert.GetTypeCode(this[pColumn]) == TypeCode.DBNull ||
                   Convert.GetTypeCode(this[pColumn]) == TypeCode.Empty;
        }

        /// <summary>
        /// Tells if a column contains a NULL value, empty string or zero.
        /// </summary>
        public bool isNullOrEmpty(string pColumn)
        {
            switch (Convert.GetTypeCode(this[pColumn]))
            {
                case TypeCode.Empty:
                case TypeCode.DBNull:
                    return true;
                case TypeCode.String:
                    return string.IsNullOrWhiteSpace(getString(pColumn));
                case TypeCode.Int16:
                    return getInt16(pColumn) == 0;
                case TypeCode.Int32:
                    return getInt32(pColumn) == 0;
                case TypeCode.Int64:
                    return getInt64(pColumn) == 0;
                case TypeCode.UInt16:
                    return getUInt16(pColumn) == 0;
                case TypeCode.UInt32:
                    return getUInt32(pColumn) == 0;
                case TypeCode.UInt64:
                    return getUInt64(pColumn) == 0;
            }
            return false;
        }
    }
}