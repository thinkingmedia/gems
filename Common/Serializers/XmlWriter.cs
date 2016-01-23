using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Common.Serializers
{
    /// <summary>
    /// Handles common serializing tasks for XML.
    /// </summary>
    public static class XmlWriter<T> where T : class
    {
        /// <summary>
        /// Writes the object to a file.
        /// </summary>
        private static void ToFile(string pFile, T pDocument, bool pLineFeeds = false)
        {
            using (StreamWriter writer = new StreamWriter(pFile, false))
            {
                ToWriter(writer, pDocument, pLineFeeds);
            }
        }

        /// <summary>
        /// Writes the object to a writer.
        /// </summary>
        private static void ToWriter(TextWriter pWriter, T pDocument, bool pLineFeeds)
        {
            // remove all namespaces
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            namespaces.Add("", "");

            XmlSerializer xml = new XmlSerializer(typeof (T));
            XmlWriterSettings settings = new XmlWriterSettings
                                         {
                                             OmitXmlDeclaration = true,
                                         };
            if (!pLineFeeds)
            {
                settings.Indent = false;
                settings.NewLineChars = "";
                settings.IndentChars = "";
            }

            using (XmlWriter writer = XmlWriter.Create(pWriter, settings))
            {
                xml.Serialize(writer, pDocument, namespaces);
            }
        }

        /// <summary>
        /// Writes the object to a file, and creates the target directory if needed.
        /// </summary>
        public static void ToFile(string pDirectory, string pFile, T pDocument, bool pLineFeeds = false)
        {
            if (!Directory.Exists(pDirectory))
            {
                Directory.CreateDirectory(pDirectory);
            }
            ToFile(pDirectory + Path.DirectorySeparatorChar + pFile, pDocument, pLineFeeds);
        }

        /// <summary>
        /// Writes the object to a string.
        /// </summary>
        public static string ToString(T pDocument, bool pLineFeeds = false)
        {
            using (StringWriter writer = new StringWriter())
            {
                ToWriter(writer, pDocument, pLineFeeds);
                return writer.ToString();
            }
        }
    }
}