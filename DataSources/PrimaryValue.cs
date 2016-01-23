using System;
using DataSources.Exceptions;

namespace DataSources
{
    /// <summary>
    /// Used to store the value of the primary key for a table.
    /// </summary>
    public sealed class PrimaryValue
    {
        /// <summary>
        /// The required type.
        /// </summary>
        private readonly Type _type;

        /// <summary>
        /// The raw value
        /// </summary>
        private object _value;

        /// <summary>
        /// True if the value is empty.
        /// </summary>
        public bool Empty
        {
            get
            {
                if (_type == typeof (UInt32))
                {
                    return ((UInt32)_value) == 0;
                }
                return (Guid)_value == Guid.Empty;
            }
        }

        /// <summary>
        /// True if a GUID type.
        /// </summary>
        public bool isGUID
        {
            get { return _type == typeof (Guid); }
        }

        /// <summary>
        /// Type if a UInt type.
        /// </summary>
        public bool isInteger
        {
            get { return _type == typeof (UInt32); }
        }

        private bool Equals(PrimaryValue pOther)
        {
            return Equals(_value, pOther._value) && _type == pOther._type;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PrimaryValue(Type pType)
        {
            _type = pType;
            if (pType == typeof (UInt32))
            {
                _value = (UInt32)0;
            }
            else if (pType == typeof (Guid))
            {
                _value = Guid.Empty;
            }
            else
            {
                throw new ModelException("Type not supported by PrimaryValue.");
            }
        }

        /// <summary>
        /// UINT constructor.
        /// </summary>
        public PrimaryValue(UInt32 pValue)
        {
            _value = pValue;
            _type = typeof (UInt32);
        }

        /// <summary>
        /// GUID constructor
        /// </summary>
        public PrimaryValue(Guid pValue)
        {
            _value = pValue;
            _type = typeof (Guid);
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        public PrimaryValue(PrimaryValue pValue)
        {
            _value = pValue._value;
            _type = pValue._type;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise,
        /// false.
        /// </returns>
        /// <param name="pObj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param>
        public override bool Equals(object pObj)
        {
            if (ReferenceEquals(null, pObj))
            {
                return false;
            }
            if (ReferenceEquals(this, pObj))
            {
                return true;
            }
            return pObj is PrimaryValue && Equals((PrimaryValue)pObj);
        }

        /// <summary>
        /// Hash code
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((_value != null ? _value.GetHashCode() : 0) * 397) ^ (_type != null ? _type.GetHashCode() : 0);
            }
        }

        /// <summary>
        /// Convert to string.
        /// </summary>
        public override string ToString()
        {
            return _value.ToString();
        }

        /// <summary>
        /// Gets the ID as GUID.
        /// </summary>
        public Guid ToGuid()
        {
            if (_type != typeof (Guid))
            {
                throw new ModelException("PrimaryValue is not a GUID type.");
            }
            return (Guid)_value;
        }

        /// <summary>
        /// Gets the ID as UINT.
        /// </summary>
        public UInt32 ToInteger()
        {
            if (_type != typeof (UInt32))
            {
                throw new ModelException("PrimaryValue is not a UINT type.");
            }
            return (UInt32)_value;
        }

        /// <summary>
        /// Returns the raw value.
        /// </summary>
        public object get()
        {
            return _value;
        }

        /// <summary>
        /// Assign the value from another primary value object.
        /// </summary>
        /// <param name="pValue"></param>
        public void set(PrimaryValue pValue)
        {
            if (_type != pValue._type)
            {
                throw new ModelException("PrimaryValues are not of the same type.");
            }
            _value = pValue._value;
        }

        /// <summary>
        /// Assigns a UINT value.
        /// </summary>
        public void set(UInt32 pValue)
        {
            if (_type != typeof (UInt32))
            {
                throw new ModelException("PrimaryValue is not a UINT type.");
            }
            _value = pValue;
        }

        /// <summary>
        /// Assigns a GUID value.
        /// </summary>
        public void set(Guid pValue)
        {
            if (_type != typeof (Guid))
            {
                throw new ModelException("PrimaryValue is not a GUID type.");
            }
            _value = pValue;
        }

        /// <summary>
        /// Checks if two IDs are equal.
        /// </summary>
        public static bool operator ==(PrimaryValue pA, PrimaryValue pB)
        {
            if (ReferenceEquals(pA, null))
            {
                return ReferenceEquals(pB, null);
            }
            if (ReferenceEquals(pB, null))
            {
                return false;
            }
            if (pA._value.GetType() == pB._value.GetType())
            {
                return pA._value == pB._value;
            }

            throw new ModelException("Can not compare PrimaryValues of different types.");
        }

        /// <summary>
        /// Checks if two IDs are not equal.
        /// </summary>
        public static bool operator !=(PrimaryValue pA, PrimaryValue pB)
        {
            if (ReferenceEquals(pA, null))
            {
                return !ReferenceEquals(pB, null);
            }
            if (ReferenceEquals(pB, null))
            {
                return true;
            }
            if (pA._value.GetType() == pB._value.GetType())
            {
                return pA._value != pB._value;
            }

            throw new ModelException("Can not compare PrimaryValues of different types.");
        }

        /// <summary>
        /// Conversion to UINT.
        /// </summary>
        public static implicit operator UInt32(PrimaryValue pA)
        {
            return pA.ToInteger();
        }

        /// <summary>
        /// Conversion UInt32 to PrimaryValue.
        /// </summary>
        public static implicit operator PrimaryValue(UInt32 pID)
        {
            return new PrimaryValue(pID);
        }

        /// <summary>
        /// Conversion UInt32 to PrimaryValue.
        /// </summary>
        public static implicit operator PrimaryValue(Guid pID)
        {
            return new PrimaryValue(pID);
        }

        /// <summary>
        /// Conversion to GUID.
        /// </summary>
        public static implicit operator Guid(PrimaryValue pA)
        {
            return pA.ToGuid();
        }

        /// <summary>
        /// Conversion to string.
        /// </summary>
        public static implicit operator string(PrimaryValue pA)
        {
            return pA._value.ToString();
        }
    }
}