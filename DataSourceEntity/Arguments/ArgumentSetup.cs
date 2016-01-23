using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSourceEntity.Arguments
{
    /// <summary>
    /// Handles configuring the command line arguments.
    /// </summary>
    public static class ArgumentSetup
    {
        /// <summary>
        /// Creates the parser.
        /// </summary>
        public static ArgumentParser Create()
        {
            ArgumentParser parser = new ArgumentParser();
            parser.Param("address", "The IP address of the MySQL server.", true, false, "localhost");
            parser.Param("database", "The database name to use.", true, true);
            parser.Param("username", "The username to use in the database connection.", true, true);
            parser.Param("password", "The password to use in the database connection.", true, true);
            parser.Param("output", "The output folder to write files.", true, true);
            parser.Param("namespace", "The C# namespace for classes.", true, true);
            parser.Param("skip", "Comma delimited list of table prefixes to ignore.", true, false);
            parser.Param("tail", "The text to add to the end of the output file's name.", true, false, "Entity");
            parser.Param("merge", "To give multiple entities a common base class. Join table names with a + and assign to a base classname using =. You can define more then one merge rule by using a comma delimiter.", true, false, "Entity");
            parser.Param("help", "Displays this help message.", false, false);

            return parser;
        }
    }
}
