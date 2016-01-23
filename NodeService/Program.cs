using System;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using Common.Log;
using Common.Log.Writers;

namespace NodeService
{
    static class Program
    {
        static EventWaitHandle _waitHandle;
        static MainService _service;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                bool runConsole = false;

                foreach (string arg in args)
                {
                    if (arg.ToLowerInvariant().Equals("-console"))
                    {
                        runConsole = true;
                    }
                }
                _service = new MainService();

                string path = Assembly.GetExecutingAssembly().Location;
                Logger.Add(new ArchiveWriter(Path.GetDirectoryName(path) + @"\logs", "server"));

                if (runConsole)
                {
                    Logger.Add(new ConsoleWriter());

                    _waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
                    Console.WriteLine("Press Ctrl+C to exit Console Mode");
                    Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);

                    _service.DebugStart();
                    WaitHandle.WaitAll(new WaitHandle[] { _waitHandle });
                }
                else
                {
                    ServiceBase.Run(_service);
                }
            }
            catch (Exception ex)
            {
                Logger.EventLog(ex);
            }
        }

        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            _service.DebugStop();
            _waitHandle.Set();
        }
    }
}
