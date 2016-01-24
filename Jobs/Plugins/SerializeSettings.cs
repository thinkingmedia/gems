using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using GemsLogger;
using Jobs.Exceptions;

namespace Jobs.Plugins
{
    public static class SerializeSettings
    {
        /// <summary>
        /// Logging
        /// </summary>
        private static readonly Logger _logger = Logger.Create(typeof (SerializeSettings));

        /// <summary>
        /// Converts properties to values. In the case of strings quotes
        /// are removed if they surround the value.
        /// </summary>
        /// <param name="pValue">Value as a string</param>
        /// <param name="pType">Target type</param>
        /// <returns>Converted to another object type</returns>
        private static object Convert(string pValue, Type pType)
        {
            if (pType == typeof (string))
            {
                pValue = Regex.Replace(pValue, "^\"(.+)\"$", "$1");
            }
            return System.Convert.ChangeType(pValue, pType);
        }

        /// <summary>
        /// Copies the values from the dictionary to the properties of the target object
        /// using the keys as property names.
        /// </summary>
        /// <param name="pTarget">The target object.</param>
        /// <param name="pValues">The properties to copy.</param>
        private static void CopyProperties(object pTarget, Dictionary<string, string> pValues)
        {
            if (pTarget == null)
            {
                throw new StorageException("Target can not be null.");
            }

            Dictionary<string, PropertyInfo> properties = pTarget
                .GetType()
                .GetProperties()
                .Where(isPublicProperty)
                .ToDictionary(pProp=>pProp.Name);

            foreach (KeyValuePair<string, string> pair in pValues)
            {
                string property = pair.Key;
                string value = pair.Value;
                if (!properties.ContainsKey(property))
                {
                    continue;
                }
                PropertyInfo info = properties[property];
                if (info.DeclaringType == null)
                {
                    continue;
                }
                object converted = Convert(value, info.PropertyType);
                info.SetValue(pTarget, converted, null);
            }
        }

        /// <summary>
        /// Serializes a [#####-####-####-###] line into a GUID
        /// </summary>
        private static Guid CreateGuid(string pStr)
        {
            return isGuid(pStr) ? new Guid(pStr.Substring(1, pStr.Length - 2)) : Guid.Empty;
        }

        /// <summary>
        /// Copies the values from the source to the properties of the target.
        /// </summary>
        /// <param name="pSource">The values loaded from the file.</param>
        /// <param name="pDest">The collection of plug-in settings.</param>
        private static void Transfer(Dictionary<Guid, PluginSettings> pDest,
                                     Dictionary<Guid, Dictionary<string, string>> pSource)
        {
            foreach (KeyValuePair<Guid, PluginSettings> dest in pDest)
            {
                if (!pSource.ContainsKey(dest.Key))
                {
                    _logger.Fine("Plug-in:{0} has no previously saved settings.", dest.Key);
                    continue;
                }
                Dictionary<string, string> source = pSource[dest.Key];

                if (!source.ContainsKey("Version"))
                {
                    _logger.Error("Settings for [{0}] missing version. Skipping.", dest.Value.Name);
                    continue;
                }

                int version = System.Convert.ToInt32(source["Version"]);
                if (dest.Value.Version != version)
                {
                    _logger.Error("[{0}] expecting version {1}, but version {2} was loaded. Skipping.", dest.Value.Name,
                        dest.Value.Version, version);
                    continue;
                }

                CopyProperties(dest.Value, source);
            }
        }

        /// <summary>
        /// Checks if a line is empty or is a comment.
        /// </summary>
        private static bool isEmptyOrComment(string pStr)
        {
            return pStr.Length == 0 || pStr.StartsWith(";");
        }

        /// <summary>
        /// Checks if a line in the file defines a GUID.
        /// </summary>
        /// <param name="pStr"></param>
        /// <returns></returns>
        private static bool isGuid(string pStr)
        {
            return pStr.Length == 38 &&
                   Regex.IsMatch(pStr, @"^\[[0-9A-F]{8}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{12}\]$",
                       RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Checks if a line is a property of GUID value.
        /// </summary>
        private static bool isGuidOrProperty(string pStr)
        {
            return isGuid(pStr) || isProperty(pStr);
        }

        /// <summary>
        /// Checks if a line represents a property assignment.
        /// </summary>
        private static bool isProperty(string pStr)
        {
            return !isEmptyOrComment(pStr) && Regex.IsMatch(pStr, @"^[0-9a-z]+=.+$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Checks if a property description defines a public property with get/set methods.
        /// </summary>
        /// <param name="pInfo"></param>
        /// <returns></returns>
        private static bool isPublicProperty(PropertyInfo pInfo)
        {
            return (pInfo.CanRead && pInfo.GetGetMethod(true).IsPublic) &&
                   (pInfo.CanWrite && pInfo.GetSetMethod(true).IsPublic);
        }

        /// <summary>
        /// Parses a string as a settings files. Converting all the data to properties
        /// grouped by GUID.
        /// </summary>
        /// <param name="pBuffer">The file contents.</param>
        /// <returns>The properties.</returns>
        private static Dictionary<Guid, Dictionary<string, string>> parse(string pBuffer)
        {
            // load all lines and skip blank or comments
            List<string> lines = pBuffer
                .Split(new[] {'\n'})
                .Select(pLine=>pLine.Trim())
                .Where(isGuidOrProperty)
                .ToList();

            // create a dictionary to hold all the values grouped by their GUID
            Dictionary<Guid, Dictionary<string, string>> groups = lines
                .Where(isGuid)
                .Select(CreateGuid)
                .ToDictionary(pGuid=>pGuid, pGuid=>new Dictionary<string, string>());

            // read the lines of text for each GUID group
            Guid current = Guid.Empty;
            foreach (string line in lines)
            {
                if (isGuid(line))
                {
                    current = CreateGuid(line);
                }
                else if (current != Guid.Empty)
                {
                    KeyValuePair<string, string> pair = property(line);
                    groups[current].Add(pair.Key, pair.Value);
                }
                else
                {
                    throw new StorageException("Unexpected line before first GUID.");
                }
            }

            return groups;
        }

        /// <summary>
        /// Reads a line from the settings file as a property.
        /// </summary>
        /// <param name="pStr">The string</param>
        /// <returns>The value split</returns>
        private static KeyValuePair<string, string> property(string pStr)
        {
            int pos = pStr.IndexOf('=');
            if (pos <= 1)
            {
                throw new StorageException("Unexpected line found. Missing = assignment.");
            }
            string key = pStr.Substring(0, pos);
            string value = pStr.Substring(pos + 1);
            return new KeyValuePair<string, string>(key, value);
        }

        /// <summary>
        /// Loads the storage from disk.
        /// </summary>
        /// <returns>True if successful</returns>
        public static bool Load(string pPath, Dictionary<Guid, PluginSettings> pTarget)
        {
            try
            {
                if (File.Exists(pPath))
                {
                    using (FileStream stream = new FileStream(pPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (TextReader reader = new StreamReader(stream))
                        {
                            string buffer = reader.ReadToEnd();
                            Dictionary<Guid, Dictionary<string, string>> source = parse(buffer);
                            Transfer(pTarget, source);
                        }
                    }
                    return true;
                }
                _logger.Error("Settings file not found. Starting with default settings.");
                _logger.Error(pPath);
            }
            catch (IOException ioException)
            {
                _logger.Exception(ioException);
            }
            return false;
        }

        /// <summary>
        /// Saves the storage to disk.
        /// </summary>
        /// <param name="pPath">The full path to the file to create.</param>
        /// <param name="pSettings">The settings to save.</param>
        public static bool Save(string pPath, Dictionary<Guid, PluginSettings> pSettings)
        {
            if (pPath == null)
            {
                throw new StorageException("Can not save before storage was loaded.");
            }
            try
            {
                using (FileStream stream = new FileStream(pPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (TextWriter writer = new StreamWriter(stream))
                    {
                        foreach (KeyValuePair<Guid, PluginSettings> pair in pSettings)
                        {
                            Guid id = pair.Key;
                            PluginSettings settings = pair.Value;

                            writer.WriteLine();
                            writer.WriteLine("; Name:{0}", settings.Name);
                            writer.WriteLine("; Type:{0}", settings.GetType().FullName);
                            writer.WriteLine("[{0}]", id);

                            List<PropertyInfo> properties = settings
                                .GetType()
                                .GetProperties()
                                .Where(isPublicProperty)
                                .ToList();

                            foreach (PropertyInfo property in properties)
                            {
                                object val = property.GetValue(settings, null);
                                if (val == null)
                                {
                                    continue;
                                }
                                string str = val is string ? string.Format("\"{0}\"", val) : val.ToString();
                                writer.WriteLine("{0}={1}", property.Name, str);
                            }
                        }
                        writer.Flush();
                        writer.Close();
                    }
                }

                _logger.Fine("Changes to settings have been saved.");
                return true;
            }
            catch (IOException ioException)
            {
                _logger.Exception(ioException);
            }
            return false;
        }
    }
}