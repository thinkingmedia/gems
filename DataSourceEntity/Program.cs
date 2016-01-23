using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Events;
using DataSourceEntity.Arguments;
using DataSourceEntity.BaseClass;
using DataSourceEntity.Models;
using DataSourceEntity.Schemas;
using DataSources.DataSource;

namespace DataSourceEntity
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the app.
        /// </summary>
        private static void Main(string[] pArgs)
        {
            Console.WriteLine(@"DataSourceEntity Version {0}, for Win64");
            Console.WriteLine(@"Copyright (c) 2013, ThinkingMedia. All rights reserved.");
            Console.WriteLine(@"Author: Mathew Foscarini, mathew@thinkingmedia.ca");
            Console.WriteLine("");
            Console.WriteLine(@"DataSourceEntity is a CLI tool to generate C# database");
            Console.WriteLine(@"entity objects from MySQL tables.");
            Console.WriteLine("");

            ArgumentParser parser = ArgumentSetup.Create();

            try
            {
                parser.Parse(pArgs);
                if (parser.Has("help"))
                {
                    parser.ShowHelp("DataSourceEntity");
                    return;
                }
                parser.Validate();

                string[] skip = parser["skip"].Split(new[] { ',' });

                SchemaReader schemaReader = new SchemaReader(parser["address"], parser["username"], parser["password"]);
                List<SchemaTable> schemas = schemaReader.Read(parser["database"], skip);

                SchemaList schema = new SchemaList(parser.Has("merge") ? parser["merge"] : null, schemas);

                TemplateWriter classImpl = new TemplateWriter("class.impl.mustache");
                TemplateWriter classBase = new TemplateWriter("class.mustache");
                SaveSchema(classImpl, schema.Schemas, parser["namespace"], parser["output"], true);
                SaveSchema(classBase, schema.Schemas, parser["namespace"], parser["output"], false);

                SaveSchema(classImpl, schema.Abstracts, parser["namespace"], parser["output"], true);
                SaveSchema(classBase, schema.Abstracts, parser["namespace"], parser["output"], false);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("");
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine("");
            }

            // done
        }

        /// <summary>
        /// Writes a schema out as a template class.
        /// </summary>
        private static void SaveSchema(
            TemplateWriter pWriter,
            IEnumerable<SchemaTable> pSchema,
            string pNameSpace,
            string pOutput,
            bool pPartial)
        {
            foreach (SchemaTable schema in pSchema)
            {
                SaveTemplate(pWriter, schema, pNameSpace, pOutput, pPartial);
            }
        }

        /// <summary>
        /// Handles writing to disk of the source code files.
        /// </summary>
        private static void SaveTemplate(
            TemplateWriter pWriter,
            SchemaTable pSchema,
            string pNameSpace,
            string pPath,
            bool pPartial)
        {
            string className = pSchema.getClassName();
            string fieldsName = pSchema.getFieldsName();
            string output = pWriter.Render(pSchema.Fields, pNameSpace, fieldsName, className, pSchema.getBaseClass(), pSchema.IsAbstract);
            string filename = string.Format(pPartial ? @"{0}.Impl.cs" : @"{0}.cs", className);
            string outfile = string.Format(@"{0}\{1}", pPath, filename);

            // don't overwrite the user file
            if (!pPartial && File.Exists(outfile))
            {
                return;
            }

            Console.WriteLine(@"File: {0}", filename);

            using (StreamWriter outStream = new StreamWriter(outfile, false))
            {
                outStream.Write(output);
                outStream.Flush();
            }
        }
    }
}