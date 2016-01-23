using System;
using System.Collections.Generic;

namespace Jobs.States
{
    /// <summary>
    /// Executes each state delay in order placed into the collection.
    /// </summary>
    public class CollectionState : List<iJobState>, iJobState
    {
        /// <summary>
        /// The current state to use from the list.
        /// </summary>
        private int _current;

        /// <summary>
        /// Constructor
        /// </summary>
        public CollectionState()
        {
            _current = 0;
        }

        /// <summary>
        /// Should the job execute tasks before the
        /// first delay period.
        /// </summary>
        /// <returns>True to run tasks first, False to delay job first.</returns>
        public bool TasksFirst()
        {
            return true;
        }

        /// <summary>
        /// Should report how long the worker thread can sleep
        /// before creating another task of tasks to be performed.
        /// </summary>
        public TimeSpan Delay()
        {
            try
            {
                return Count == 0 ? TimeSpan.Zero : this[_current].Delay();
            }
            finally
            {
                _current = _current + 1 == Count ? 0 : _current + 1;
            }
        }

        /// <summary>
        /// Checks if the job is finished and can be shut down.
        /// </summary>
        public bool isFinished()
        {
            return Count == 0;
        }

        /// <summary>
        /// Adds the child state, unless it's a collection which are merged.
        /// </summary>
        public new void Add(iJobState pState)
        {
            CollectionState collection = pState as CollectionState;
            if (collection == null)
            {
                base.Add(pState);
                return;
            }

            foreach (iJobState state in collection)
            {
                Add(state);
            }
        }

        /// <summary>
        /// Adds a state multiple times to the collection.
        /// </summary>
        public void AddMany(iJobState pState, int pCount)
        {
            for (int i = 0; i < pCount; i++)
            {
                Add(pState);
            }
        }
    }
}