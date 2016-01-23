using System;

namespace Common.HeartBeat
{
    /// <summary>
    /// Defines a resource that is monitored by HeartBeat.
    /// </summary>
    class HeartBeatResource
    {
        /// <summary>
        /// The _id of this resource.
        /// </summary>
        public String ID { get; private set; }

        /// <summary>
        /// How long it can live without a beat.
        /// </summary>
        private readonly int _lifeSpan;

        /// <summary>
        /// Tells if the Monitor has been notified that this resource is dead.
        /// </summary>
        public bool Notified { get; set; }

        /// <summary>
        /// The last time a beat was received.
        /// </summary>
        private DateTime _lastBeat;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="pID"></param>
        /// <param name="pLifeSpan"></param>
        public HeartBeatResource(String pID, int pLifeSpan)
        {
            ID = pID;
            _lifeSpan = pLifeSpan;
            Beat();
        }

        /// <summary>
        /// Performs a beat to keep the resource alive.
        /// </summary>
        public void Beat()
        {
            _lastBeat = DateTime.Now;
            Notified = false;
        }

        /// <summary>
        /// Checks if this resource has died because it did not receive a beat
        /// within the allowed LifeSpan.
        /// </summary>
        /// <returns></returns>
        public bool isDead()
        {
            TimeSpan tSpan = DateTime.Now - _lastBeat;

            return tSpan.TotalMilliseconds > _lifeSpan;
        }
    }
}
