using System.IO;
using System.Xml.Serialization;

namespace Common.Serializers
{
    /// <summary>
    /// Handles common reading methods for serializing XML.
    /// </summary>
    public static class XmlReader<T> where T : class
    {
        /// <summary>
        /// Creates an object from a string.
        /// </summary>
        public static T FromString(string pSource)
        {
            using (StringReader reader = new StringReader(pSource))
            {
                return FromReader(reader);
            }
        }

        /// <summary>
        /// Creates an object from a file.
        /// </summary>
        public static T FromFile(string pFile)
        {
            using (FileStream file = new FileStream(pFile, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    return FromReader(reader);
                }
            }
        }

        /// <summary>
        /// Creates an object from a reader.
        /// </summary>
        public static T FromReader(TextReader pReader)
        {
            XmlSerializer reader = new XmlSerializer(typeof(T));
            return reader.Deserialize(pReader) as T;
        }
    }
}
