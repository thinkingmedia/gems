using System;
using System.ComponentModel;
using System.Windows.Forms;
using Logging;

namespace Common.Events
{
    /// <summary>
    /// Used to fire events without worrying about handlers being attached to the object.
    /// </summary>
    public static class FireEvents
    {
        /// <summary>
        /// Log
        /// </summary>
        private static readonly Logger _logger = Logger.Create(typeof (FireEvents));

        /// <summary>
        /// Fires an event with no arguments.
        /// </summary>
        public static void Action(Action pHandler)
        {
            if (pHandler == null)
            {
                return;
            }

            try
            {
                pHandler();
            }
            catch (Exception e)
            {
                _logger.Exception(e);
            }
        }

        /// <summary>
        /// Fires an event with one argument.
        /// </summary>
        public static void Action<TArgType>(Action<TArgType> pHandler, TArgType pArg)
        {
            if (pHandler == null)
            {
                return;
            }

            try
            {
                pHandler(pArg);
            }
            catch (Exception e)
            {
                _logger.Exception(e);
            }
        }

        /// <summary>
        /// Fires an action with two arguments.
        /// </summary>
        public static void Action<TArg1Type, TArg2Type>(Action<TArg1Type, TArg2Type> pHandler, TArg1Type pArg1,
                                                        TArg2Type pArg2)
        {
            if (pHandler == null)
            {
                return;
            }

            try
            {
                pHandler(pArg1, pArg2);
            }
            catch (Exception e)
            {
                _logger.Exception(e);
            }
        }

        /// <summary>
        /// Fires an action with three arguments.
        /// </summary>
        public static void Action<TArg1Type, TArg2Type, TArg3Type>(Action<TArg1Type, TArg2Type, TArg3Type> pHandler,
                                                                   TArg1Type pArg1, TArg2Type pArg2, TArg3Type pArg3)
        {
            if (pHandler == null)
            {
                return;
            }

            try
            {
                pHandler(pArg1, pArg2, pArg3);
            }
            catch (Exception e)
            {
                _logger.Exception(e);
            }
        }

        /// <summary>
        /// Fires an empty event safely.
        /// </summary>
        public static void Empty(object pSender, EventHandler pHandler)
        {
            if (pHandler == null)
            {
                return;
            }
            try
            {
                pHandler(pSender, EventArgs.Empty);
            }
            catch (Exception e)
            {
                _logger.Exception(e);
            }
        }

        /// <summary>
        /// Only calls BeginInvoke if it's required.
        /// </summary>
        public static void Invoke(Control pControl, Action pAction)
        {
            if (pControl.InvokeRequired)
            {
                pControl.BeginInvoke(pAction);
            }
            else
            {
                pAction();
            }
        }

        /// <summary>
        /// Fires a property changed event for objects that implement INotifyPropertyChanged
        /// </summary>
        /// <param name="pOwner">The object</param>
        /// <param name="pHandler">PropertyChanged property reference</param>
        /// <param name="pPropertyName">Name of the property</param>
        public static void PropertyChanged(INotifyPropertyChanged pOwner, PropertyChangedEventHandler pHandler,
                                           string pPropertyName)
        {
            if (pHandler != null)
            {
                pHandler(pOwner, new PropertyChangedEventArgs(pPropertyName));
            }
        }

        /// <summary>
        /// Fires an argument event safely.
        /// </summary>
        public static void WithArgs<TArgType>(object pSender, EventHandler pHandler, TArgType pArg)
        {
            if (pHandler == null)
            {
                return;
            }
            try
            {
                pHandler(pSender, new FireEventArgs<TArgType>(pArg));
            }
            catch (Exception e)
            {
                _logger.Exception(e);
            }
        }
    }
}