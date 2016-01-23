using System;
using System.Collections.Generic;
using System.Reflection;

namespace Common.Events
{
    /// <summary>
    /// A class used to handle the merging of an object's properties.
    /// </summary>
    public static class PropertyMerger
    {
        /// <summary>
        /// Creates a new object that contains the properties of the two objects merged together.
        /// </summary>
        /// <typeparam name="T">The class type to merge.</typeparam>
        /// <param name="pDefaults">Instance of the defaults object.</param>
        /// <param name="pSettings">Instance of the settings object.</param>
        /// <param name="pMergeFields"></param>
        /// <param name="pMergeProperties"></param>
        /// <returns>A new instance of T with the merged results.</returns>
        public static T ObjectMerge<T>(T pDefaults, T pSettings, bool pMergeFields = true, bool pMergeProperties = true) where T : class, new()
        {
            T target = new T();
            Type type = typeof(T);
            List<MemberInfo> infos = new List<MemberInfo>(type.GetMembers());

            foreach (MemberInfo info in infos)
            {
                // Copy values from either defaults or settings
                if (pMergeFields && info.MemberType == MemberTypes.Field)
                {
                    FieldInfo field = (FieldInfo)info;
                    if (field.IsPublic)
                    {
                        object v1 = field.GetValue(pSettings);
                        v1 = v1 ?? field.GetValue(pDefaults);
                        field.SetValue(target, v1);
                    }
                }

                // Copy values from either defaults or settings
                if (!pMergeProperties || info.MemberType != MemberTypes.Property)
                {
                    continue;
                }

                PropertyInfo prop = (PropertyInfo)info;
                if (!prop.CanWrite || !prop.CanRead)
                {
                    continue;
                }

                object v2 = prop.GetValue(pSettings, null);
                v2 = v2 ?? prop.GetValue(pDefaults, null);
                prop.SetValue(target, v2, null);
            }

            return target;
        }
    }
}