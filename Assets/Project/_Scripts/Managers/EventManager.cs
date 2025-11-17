using System;
using System.Collections.Generic;
using MoveStopMove.Extensions.Observer;
using MoveStopMove.Extensions.Singleton;

namespace MoveStopMove.Managers
{
    public class EventManager : Singleton<EventManager>
    {
        #region -- Fields --

        private static readonly Dictionary<Type, List<object>> s_observers = new();

        #endregion

        #region -- Methods --

        public void Subscribe<T>(IMyObserver<T> observer)
        {
            var type = typeof(T);
            if (!s_observers.ContainsKey(type))
                s_observers[type] = new List<object>();

            if (!s_observers[type].Contains(observer))
                s_observers[type].Add(observer);
        }

        public void Unsubscribe<T>(IMyObserver<T> observer)
        {
            var type = typeof(T);
            if (!s_observers.TryGetValue(type, out var list))
                return;

            list.Remove(observer);

            if (list.Count == 0)
                s_observers.Remove(type);
        }

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