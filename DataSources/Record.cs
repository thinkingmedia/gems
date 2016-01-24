using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Common.Annotations;
using DataSources.Query;
using GemsLogger;
using MySql.Data.Types;

namespace DataSources
{
    /// <summary>
    /// Holds data associated with a record in the database.
    /// </summary>
    public class Record : iRecord
    {
        /// <summary>
        /// Logging
        /// </summary>
        private static readonly Logger _logger = Logger.Create(typeof (Record));

        /// <summary>
        /// The default table.
        /// </summary>
        private readonly string _default;

        /// <summary>
        /// Data is organized by table.
        /// </summary>
        private readonly Dictionary<string, RecordValue> _tables;

        /// <summary>
        /// The number of fields.
        /// </summary>
        public int Count
        {
            get { return _tables[_default].Count; }
        }

        /// <summary>
        /// Assumes the ID is a UINT.
        /// </summary>
        public UInt32 ID
        {
            get { return _tables[_default].getUInt32("id"); }
            set { _tables[_default]["id"] = value; }
        }

        /// <summary>
        /// Access the fields by fieldname.
        /// </summary>
        public object this[string pKey]
        {
            get { return _tables[_default][pKey]; }
            set { _tables[_default][pKey] = value; }
        }

        /// <summary>
        /// Access the fields by offset.
        /// </summary>
        public object this[int pIndx]
        {
            get { return _tables[_default][pIndx]; }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Record()
            : this("")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Record([NotNull] Model pModel)
            : this(pModel.Settings.Table)
        {
            if (pModel == null)
            {
                throw new ArgumentNullException("pModel");
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private Record([NotNull] string pTable)
        {
            if (pTable == null)
            {
                throw new ArgumentNullException("pTable");
            }

            _default = pTable;
            _tables = new Dictionary<string, RecordValue>
                      {
                          {_default, new RecordValue()}
                      };
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Record([NotNull] string pTable, [NotNull] RecordValue pValue)
        {
            if (pTable == null)
            {
                throw new ArgumentNullException("pTable");
            }
            if (pValue == null)
            {
                throw new ArgumentNullException("pValue");
            }
            _default = pTable;
            _tables = new Dictionary<string, RecordValue>
                      {
                          {_default, pValue}
                      };
        }

        /// <summary>
        /// Reads a row from the database.
        /// </summary>
        public Record([NotNull] IDataRecord pReader)
            : this()
        {
            if (pReader == null)
            {
                throw new ArgumentNullException("pReader");
            }
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
        public Record([NotNull] Record pRecord)
        {
            if (pRecord == null)
            {
                throw new ArgumentNullException("pRecord");
            }
            _default = pRecord._default;
            _tables = new Dictionary<string, RecordValue>();
            foreach (KeyValuePair<string, RecordValue> pair in pRecord._tables)
            {
                _tables.Add(pair.Key, new RecordValue(pair.Value));
            }
        }

        /// <summary>
        /// Tells if the a fieldname exists.
        /// </summary>
        public bool Has([NotNull] string pKey)
        {
            if (pKey == null)
            {
                throw new ArgumentNullException("pKey");
            }
            return _tables[_default].Has(pKey);
        }

        /// <summary>
        /// Removes a field.
        /// </summary>
        public void Remove([NotNull] string pKey)
        {
            if (pKey == null)
            {
                throw new ArgumentNullException("pKey");
            }
            _tables[_default].Remove(pKey);
        }

        /// <summary>
        /// Assigns the values of this record to a query.
        /// </summary>
        public void Set([NotNull] iQueryBuilder pQuery)
        {
            if (pQuery == null)
            {
                throw new ArgumentNullException("pQuery");
            }
            _tables[_default].Set(pQuery);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public byte[] getBlob([NotNull] string pColumn)
        {
            if (pColumn == null)
            {
                throw new ArgumentNullException("pColumn");
            }
            return _tables[_default].getBlob(pColumn);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public bool getBoolean([NotNull] string pColumn)
        {
            if (pColumn == null)
            {
                throw new ArgumentNullException("pColumn");
            }
            return _tables[_default].getBoolean(pColumn);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public byte getByte([NotNull] string pColumn)
        {
            if (pColumn == null)
            {
                throw new ArgumentNullException("pColumn");
            }
            return _tables[_default].getByte(pColumn);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public char getChar([NotNull] string pColumn)
        {
            if (pColumn == null)
            {
                throw new ArgumentNullException("pColumn");
            }
            return _tables[_default].getChar(pColumn);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public DateTime getDateTime([NotNull] string pColumn)
        {
            if (pColumn == null)
            {
                throw new ArgumentNullException("pColumn");
            }
            return _tables[_default].getDateTime(pColumn);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public decimal getDecimal([NotNull] string pColumn)
        {
            if (pColumn == null)
            {
                throw new ArgumentNullException("pColumn");
            }
            return _tables[_default].getDecimal(pColumn);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public double getDouble([NotNull] string pColumn)
        {
            if (pColumn == null)
            {
                throw new ArgumentNullException("pColumn");
            }
            return _tables[_default].getDouble(pColumn);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public float getFloat([NotNull] string pColumn)
        {
            if (pColumn == null)
            {
                throw new ArgumentNullException("pColumn");
            }
            return _tables[_default].getFloat(pColumn);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public Guid getGuid([NotNull] string pColumn)
        {
            if (pColumn == null)
            {
                throw new ArgumentNullException("pColumn");
            }
            return _tables[_default].getGuid(pColumn);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public short getInt16([NotNull] string pColumn)
        {
            if (pColumn == null)
            {
                throw new ArgumentNullException("pColumn");
            }
            return _tables[_default].getInt16(pColumn);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public int getInt32([NotNull] string pColumn)
        {
            if (pColumn == null)
            {
                throw new ArgumentNullException("pColumn");
            }
            return _tables[_default].getInt32(pColumn);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public long getInt64([NotNull] string pColumn)
        {
            if (pColumn == null)
            {
                throw new ArgumentNullException("pColumn");
            }
            return _tables[_default].getInt64(pColumn);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public string getString([NotNull] string pColumn)
        {
            if (pColumn == null)
            {
                throw new ArgumentNullException("pColumn");
            }
            return _tables[_default].getString(pColumn);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public ushort getUInt16([NotNull] string pColumn)
        {
            if (pColumn == null)
            {
                throw new ArgumentNullException("pColumn");
            }
            return _tables[_default].getUInt16(pColumn);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public UInt32 getUInt32([NotNull] string pColumn)
        {
            if (pColumn == null)
            {
                throw new ArgumentNullException("pColumn");
            }
            return _tables[_default].getUInt32(pColumn);
        }

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        public ulong getUInt64([NotNull] string pColumn)
        {
            if (pColumn == null)
            {
                throw new ArgumentNullException("pColumn");
            }
            return _tables[_default].getUInt64(pColumn);
        }

        /// <summary>
        /// Tells if a column contains a NULL value.
        /// </summary>
        public bool isNull([NotNull] string pColumn)
        {
            if (pColumn == null)
            {
                throw new ArgumentNullException("pColumn");
            }
            return _tables[_default].isNull(pColumn);
        }

        /// <summary>
        /// Tells if a column contains a NULL value, empty string or zero.
        /// </summary>
        public bool isNullOrEmpty([NotNull] string pColumn)
        {
            if (pColumn == null)
            {
                throw new ArgumentNullException("pColumn");
            }
            return _tables[_default].isNullOrEmpty(pColumn);
        }

        /// <summary>
        /// Access a record for a table.
        /// </summary>
        public RecordValue Get([NotNull] string pTable)
        {
            if (pTable == null)
            {
                throw new ArgumentNullException("pTable");
            }
            string table = string.IsNullOrWhiteSpace(pTable) ? _default : pTable;

            if (!_tables.ContainsKey(table))
            {
                _tables.Add(table, new RecordValue());
            }
            return _tables[table];
        }

        /// <summary>
        /// A list of inner aliases.
        /// </summary>
        public IEnumerable<string> getAliases()
        {
            return _tables.Keys;
        }

        /// <summary>
        /// Gets the inner values for an alias.
        /// </summary>
        public RecordValue getValues([NotNull] string pAlias)
        {
            if (pAlias == null)
            {
                throw new ArgumentNullException("pAlias");
            }
            return _tables[pAlias];
        }
    }
}