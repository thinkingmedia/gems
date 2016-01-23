using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Common.Annotations;
using DataSourceEntity.Schemas;
using Nustache.Core;

namespace DataSourceEntity
{
    /// <summary>
    /// Writes the template to disk.
    /// </summary>
    public class TemplateWriter
    {
        /// <summary>
        /// The assembly to load files from.
        /// </summary>
        private readonly Assembly _files;

        /// <summary>
        /// The name of the application. Used to load embedded files.
        /// </summary>
        private readonly string _packageName;

        /// <summary>
        /// The Mustache template
        /// </summary>
        private readonly Template _template;

        /// <summary>
        /// Renders the template using the data.
        /// </summary>
        private string RenderTemplate(IDictionary<string, object> pData)
        {
            StringWriter outWriter = new StringWriter();
            _template.Render(pData, outWriter, null, null);
            return outWriter.ToString();
        }

        /// <summary>
        /// Loads a resource from the application as a string.
        /// </summary>
        private string getTemplateAsString(string pResourceName)
        {
            string fullResourceName = string.Format("{0}.Templates.{1}", _packageName, pResourceName);
            using (Stream stream = _files.GetManifestResourceStream(fullResourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException("Did you forget to embed the resource via it's properties?");
                }
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateWriter(string pTemplateName)
        {
            _packageName = GetType().FullName.Split(new[] {'.'})[0];
            _files = Assembly.GetCallingAssembly();

            _template = new Template();
            _template.Load(new StringReader(getTemplateAsString(pTemplateName)));
        }

        /// <summary>
        /// Renders the template for the entity.
        /// </summary>
        public string Render([NotNull] IEnumerable<SchemaField> pSchemas, [NotNull] string pNameSpace,
                             [NotNull] string pFieldsName, [NotNull] string pClassName, [NotNull] string pBaseClass,
                             bool pAbstract)
        {
            if (pSchemas == null)
            {
                throw new ArgumentNullException("pSchemas");
            }
            if (pNameSpace == null)
            {
                throw new ArgumentNullException("pNameSpace");
            }
            if (pFieldsName == null)
            {
                throw new ArgumentNullException("pFieldsName");
            }
            if (pClassName == null)
            {
                throw new ArgumentNullException("pClassName");
            }
            if (pBaseClass == null)
            {
                throw new ArgumentNullException("pBaseClass");
            }

            Dictionary<string, object> data = new Dictionary<string, object>
                                              {
                                                  {"NameSpace", pNameSpace},
                                                  {"FieldsName", pFieldsName},
                                                  {"ClassName", pClassName},
                                                  {"BaseClass", pBaseClass},
                                                  {"Abstract", pAbstract}
                                              };
            List<Dictionary<string, object>> fields = pSchemas.Select(pSchema=>pSchema.getTemplateArguments()).ToList();
            data.Add("Fields", fields);

            return RenderTemplate(data);
        }
    }
}