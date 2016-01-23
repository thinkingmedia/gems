using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using gems_collections.List;
using Jobs.States;

namespace Jobs.Tasks.Events
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class EventRecorder : iEventRecorder
    {
        /// <summary>
        /// Counts the occurrence of events.
        /// </summary>
        private readonly Dictionary<eEVENT_SEVERITY, int> _counters;

        /// <summary>
        /// A list of past exceptions.
        /// </summary>
        private readonly LimitedList<iEventObject> _events;

        /// <summary>
        /// Constructor
        /// </summary>
        public EventRecorder(int pLimit)
        {
            _events = new LimitedList<iEventObject>(pLimit);
            _counters = new Dictionary<eEVENT_SEVERITY, int>();
        }

        /// <summary>
        /// Records an event.
        /// </summary>
        public void Add(iEventObject pEvent)
        {
            lock (_events)
            {
                lock (_counters)
                {
                    _events.Add(pEvent);

                    if (!_counters.ContainsKey(pEvent.Severity))
                    {
                        _counters.Add(pEvent.Severity, 0);
                    }
                    _counters[pEvent.Severity]++;
                }
            }
        }

        /// <summary>
        /// Clears the event recorder history.
        /// </summary>
        public void Clear()
        {
            lock (_counters)
            {
                _counters.Clear();
            }
            lock (_events)
            {
                _events.Clear();
            }
        }

        /// <summary>
        /// Counts how many events in total has occurred.
        /// </summary>
        public int getCount()
        {
            lock (_counters)
            {
                return _counters.Values.Sum();
            }
        }

        /// <summary>
        /// Counts how many events in total have occurred by type.
        /// </summary>
        public int getCount(eEVENT_SEVERITY pSeverity)
        {
            lock (_counters)
            {
                if (_counters.ContainsKey(pSeverity))
                {
                    return _counters[pSeverity];
                }
            }
            return 0;
        }

        /// <summary>
        /// Gets a history of past events.
        /// </summary>
        public List<iEventObject> getEvents()
        {
            lock (_events)
            {
                return new List<iEventObject>(_events);
            }
        }
    }
}