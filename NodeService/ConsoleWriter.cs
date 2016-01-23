using System;
using Common.Log;

namespace NodeService
{
    /// <summary>
    /// Used to write the log output to the console.
    /// </summary>
    public class ConsoleWriter : ILogWriter
    {
        public void Open()
        {
        }

        public void Write(Logger.LogLevel pLevel, string pPrefix, string pMsg)
        {
            Console.WriteLine(pMsg);
        }

        public void Close()
        {
        }
    }
}
