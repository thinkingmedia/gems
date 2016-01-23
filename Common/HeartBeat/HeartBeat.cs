using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Logging;

namespace Common.HeartBeat
{
    /// <summary>
    /// Heartbeat will Monitor resources and keep them alive as long as that resource
    /// receives regular beats.
    /// </summary>
    public class HeartBeat
    {
        private static readonly Logger _logger = Logger.Create(typeof(HeartBeat));

        public delegate void HeartBeatHandler(object pSender, String pID, bool pDead);
        public event HeartBeatHandler HeartBeatEvent;

        /// <summary>
        /// A list of resources.
        /// </summary>
        private readonly Dictionary<String, HeartBeatResource> _resources = new Dictionary<string, HeartBeatResource>();

        /// <summary>
        /// The thread that will Monitor the life of resources.
        /// </summary>
        private readonly Thread _worker;

        /// <summary>
        /// Constructor
        /// </summary>
        public HeartBeat()
        {
            _worker = new Thread(Monitor) { Name = "HeartBeat", IsBackground = true };
            _worker.Start();
        }

        /// <summary>
        /// Tells the HeartBeat thread to exit.
        /// </summary>
        public void Stop()
        {
            _worker.Interrupt();
        }

        /// <summary>
        /// Creates a resource that should remain alive by calling Beat(id).
        /// </summary>
        /// <param name="pID">A unique _name for this resource.</param>
        /// <param name="pLifespan">The number of milliseconds this resource can remain alive without a heartbeat.</param>
        public void Add(string pID, int pLifespan)
        {
            Remove(pID);

            _logger.Debug("Adding:{0} Life:{1}", pID, pLifespan);

            lock (_resources)
            {
                _resources.Add(pID, new HeartBeatResource(pID, pLifespan));
            }
        }

        /// <summary>
        /// Removes a resource from being monitored.
        /// </summary>
        /// <param name="pID"></param>
        public void Remove(string pID)
        {
            lock (_resources)
            {
                if (_resources.ContainsKey(pID))
                {
                    _logger.Debug("Removing:{0}", pID);
                    _resources.Remove(pID);
                }
            }
        }

        /// <summary>
        /// Tells if HeartBeat is monitoring a resource _id.
        /// </summary>
        public bool Contains(string pID)
        {
            lock (_resources)
            {
                return _resources.ContainsKey(pID);
            }
        }

        /// <summary>
        /// Clears the list of resources being monitored.
        /// </summary>
        public void Clear()
        {
            lock (_resources)
            {
                _resources.Clear();
            }
        }

        /// <summary>
        /// Reports that the given resource _id is still alive.
        /// </summary>
        /// <param name="pID">The resource to keep alive.</param>
        public void Beat(string pID)
        {
            lock (_resources)
            {
                if (_resources.ContainsKey(pID))
                {
                    // notify the Monitor that the resource has come back to life.
                    if (_resources[pID].Notified)
                    {
                        if (HeartBeatEvent != null)
                        {
                            HeartBeatEvent(this, pID, false);
                        }
                    }

                    _resources[pID].Beat();
                }
                else
                {
                    _logger.Error("Can't beat unknown resource {0}", pID);
                }
            }
        }

        /// <summary>
        /// The monitoring thread that watches if a resource appears to have died.
        /// </summary>
        private void Monitor()
        {
            _logger.Fine("HeartBeat has started.");

            bool running = true;
            while (running)
            {
                try
                {
                    // check the life of a resource every second
                    Thread.Sleep(1000);

                    lock (_resources)
                    {
                        foreach (HeartBeatResource resource in _resources.Values.Where(pResource => pResource.isDead() && !pResource.Notified))
                        {
                            // only send the notification once.
                            resource.Notified = true;

                            if (HeartBeatEvent != null)
                            {
                                HeartBeatEvent(this, resource.ID, true);
                            }
                        }
                    }
                }
                catch (ThreadInterruptedException)
                {
                    running = false;
                }
            }

            _logger.Fine("HeartBeat has stopped.");
        }
    }
}
