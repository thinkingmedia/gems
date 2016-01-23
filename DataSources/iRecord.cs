using System;
using DataSources.Query;

namespace DataSources
{
    public interface iRecord
    {
        /// <summary>
        /// Tells if the a fieldname exists.
        /// </summary>
        bool Has(string pKey);

        /// <summary>
        /// Removes a field.
        /// </summary>
        void Remove(string pKey);

        /// <summary>
        /// Assigns the values of this record to a query.
        /// </summary>
        void Set(iQueryBuilder pQuery);

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        byte[] getBlob(string pColumn);

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        bool getBoolean(string pColumn);

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        byte getByte(string pColumn);

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        char getChar(string pColumn);

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        DateTime getDateTime(string pColumn);

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        decimal getDecimal(string pColumn);

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        double getDouble(string pColumn);

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        float getFloat(string pColumn);

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        Guid getGuid(string pColumn);

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        short getInt16(string pColumn);

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        int getInt32(string pColumn);

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        long getInt64(string pColumn);

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        string getString(string pColumn);

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        ushort getUInt16(string pColumn);

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        UInt32 getUInt32(string pColumn);

        /// <summary>
        /// Read the column as a given type.
        /// </summary>
        ulong getUInt64(string pColumn);

        /// <summary>
        /// Tells if a column contains a NULL value.
        /// </summary>
        bool isNull(string pColumn);

        /// <summary>
        /// Tells if a column contains a NULL value, empty string or zero.
        /// </summary>
        bool isNullOrEmpty(string pColumn);
    }
}