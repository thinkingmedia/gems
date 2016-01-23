using System;
using System.Linq;
using System.Reflection;
using Common.Exceptions;

namespace Common.Utils
{
    /// <summary>
    /// Methods to assist in the modifying of classes and methods at runtime.
    /// </summary>
    public static class Reflection
    {
        /// <summary>
        /// Finds the class Type and ensures that it can be created using a
        /// default constructor.
        /// </summary>
        /// <param name="pClassName">The class name.</param>
        /// <param name="pArgTypes">(optional) list of types the constructor must have.</param>
        /// <param name="pQuite">Disable exceptions if invalid type.</param>
        /// <returns>The class Type, or Null if quite mode and invalid type.</returns>
        public static Type getCreatableType(string pClassName, Type[] pArgTypes = null, bool pQuite = false)
        {
            Type type = Type.GetType(pClassName);
            if (type == null)
            {
                if (pQuite)
                {
                    return null;
                }
                throw new ReflectionException(string.Format("Class type not found: {0}. Did you forget to define the assembly?", pClassName));
            }

            if (!pQuite && type.GetConstructor(pArgTypes ?? Type.EmptyTypes) == null)
            {
                throw new ReflectionException("{0} does not have a constructor that takes {1} arguments.", pClassName, pArgTypes == null ? 0 : pArgTypes.Length);
            }

            return type;
        }

        /// <summary>
        /// Uses the getCreatableType method and creates a new instance of the type.
        /// </summary>
        /// <typeparam name="T">The type to cast the object too. Usually a base class or interface.</typeparam>
        /// <param name="pClassName">The class name.</param>
        /// <param name="pArgs">The arguments for the constructor.</param>
        /// <returns>The new object instance.</returns>
        public static T Create<T>(string pClassName, params object[] pArgs) where T : class
        {
            Type[] types = pArgs.Select(pArg => pArg.GetType()).ToArray();
            Type type = getCreatableType(pClassName, types.Length == 0 ? null : types);
            return Activator.CreateInstance(type, pArgs) as T;
        }

        /// <summary>
        /// Returns the value of a property using a string to identify the property name.
        /// </summary>
        /// <typeparam name="T">The property type.</typeparam>
        /// <param name="pObject">The object ref</param>
        /// <param name="pPropertyName">The property name</param>
        /// <returns></returns>
        public static T getProperty<T>(object pObject, string pPropertyName) where T : struct
        {
            if (pObject == null || pPropertyName == null)
            {
                return default(T);
            }
            PropertyInfo info = pObject.GetType().GetProperty(pPropertyName);
            object value = info.GetValue(pObject, null);
            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// Assigns the value to a property using a string to identify the property. The value
        /// can be of a different type if conversion is possible.
        /// </summary>
        /// <typeparam name="T">The property type</typeparam>
        /// <param name="pObject">The object ref</param>
        /// <param name="pPropertyName">The name of the property</param>
        /// <param name="pValue">Value for the property</param>
        public static void setProperty<T>(object pObject, string pPropertyName, T pValue) where T : struct
        {
            if (pObject == null || pPropertyName == null)
            {
                return;
            }
            PropertyInfo info = pObject.GetType().GetProperty(pPropertyName);
            if (typeof(T) != info.PropertyType)
            {
                T converted = (T)Convert.ChangeType(pValue, info.PropertyType);
                info.SetValue(pObject, converted, null);
            }
            else
            {
                info.SetValue(pObject, pValue, null);
            }
        }
    }
}
