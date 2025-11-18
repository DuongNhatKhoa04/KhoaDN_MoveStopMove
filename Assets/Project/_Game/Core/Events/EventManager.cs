using MoveStopMove.Core.Interfaces;
using MoveStopMove.Utility.Extension;
using System;
using System.Collections.Generic;

namespace MoveStopMove.Core.Events
{
    public class EventManager : Singleton<EventManager>
    {
        #region -- Fields --

        private static readonly Dictionary<Type, List<object>> s_observers = new();

        #endregion

        #region -- Methods --

        /// <summary>
        /// Subscribe to get envent
        /// </summary>
        /// <param name="observer">Observer of type</param>
        /// <typeparam name="T">Data type</typeparam>
        public void Subscribe<T>(IMyObserver<T> observer)
        {
            var type = typeof(T);
            if (!s_observers.ContainsKey(type))
                s_observers[type] = new List<object>();

            if (!s_observers[type].Contains(observer))
                s_observers[type].Add(observer);
        }

        /// <summary>
        /// Unsubscribe to not get event
        /// </summary>
        /// <param name="observer">Observer of type</param>
        /// <typeparam name="T">Data type</typeparam>
        public void Unsubscribe<T>(IMyObserver<T> observer)
        {
            var type = typeof(T);
            if (!s_observers.TryGetValue(type, out var list))
                return;

            list.Remove(observer);

            if (list.Count == 0)
                s_observers.Remove(type);
        }

        /// <summary>
        /// Notify event to each listener
        /// </summary>
        /// <param name="data">Data</param>
        /// <typeparam name="T">Data type</typeparam>
        public void Notify<T>(T data)
        {
            var type = typeof(T);
            if (!s_observers.TryGetValue(type, out var list)) return;

            for (int i = list.Count - 1; i >= 0; i--)
            {
                ((IMyObserver<T>)list[i]).OnNotify(data);
            }
        }

        public static void Clear() => s_observers.Clear();

        #endregion
    }
}