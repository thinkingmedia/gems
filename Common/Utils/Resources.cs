using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using Common.Exceptions;

namespace Common.Utils
{
    /// <summary>
    /// Handles serializing resources embedded in the assembly of the application.
    /// </summary>
    public class Resources
    {
        /// <summary>
        /// The assembly to load files from.
        /// </summary>
        private readonly Assembly _files;

        /// <summary>
        /// The folder resources are stored in.
        /// </summary>
        private readonly string _folder;

        /// <summary>
        /// The name of the package used to load embedded files.
        /// </summary>
        private readonly string _packageName;

        /// <summary>
        /// Checks that a stream object is not null.
        /// </summary>
        private static CommonException CreateNotFound(string pFullResourceName)
        {
            return new CommonException(
                "Resource not found {0} - Did you forget to embed the resource via it's properties?",
                pFullResourceName);
        }

        /// <summary>
        /// Generates the name of a resource.
        /// </summary>
        private string getFullResourceName(string pResourceName)
        {
            return string.Format("{0}.{1}.{2}", _packageName, _folder, pResourceName);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Resources(Type pOwner, string pFolder)
        {
            _packageName = pOwner.FullName.Split(new[] {'.'})[0];
            _files = pOwner.Assembly;
            _folder = pFolder;
        }

        /// <summary>
        /// Loads a resource from the application manifest as a Bitmap
        /// </summary>
        /// <param name="pResourceName"></param>
        /// <returns></returns>
        public Bitmap ReadAsBitmap(string pResourceName)
        {
            using (Stream stream = getStream(pResourceName))
            {
                if (stream == null)
                {
                    throw CreateNotFound(getFullResourceName(pResourceName));
                }
                return new Bitmap(stream);
            }
        }

        /// <summary>
        /// Loads a resource from the application manifest as a string.
        /// </summary>
        public string ReadAsString(string pResourceName, bool pStripReturns = true, bool pForceUTF8 = false)
        {
            using (Stream stream = getStream(pResourceName))
            {
                if (stream == null)
                {
                    throw CreateNotFound(getFullResourceName(pResourceName));
                }
                using (StreamReader reader = new StreamReader(stream))
                {
                    string str = reader.ReadToEnd();
                    if (pForceUTF8)
                    {
                        byte[] bytes = Encoding.Default.GetBytes(str);
                        str = Encoding.UTF8.GetString(bytes);
                    }
                    if (pStripReturns)
                    {
                        str = str.Replace("\r", "");
                    }
                    return str;
                }
            }
        }

        /// <summary>
        /// Creates a stream for the resource.
        /// </summary>
        public Stream getStream(string pResourceName)
        {
            string fullResourceName = getFullResourceName(pResourceName);
            return _files.GetManifestResourceStream(fullResourceName);
        }
    }
}