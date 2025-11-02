using System;
using System.Collections.Generic;
using MoveStopMove.Extensions.Observer;

namespace MoveStopMove.Managers
{
    public static class EventManager
    {
        #region -- Fields --

        private static readonly Dictionary<Type, List<object>> s_Observers = new();

        #endregion

        #region -- Methods --

        public static void Subscribe<T>(IMyObserver<T> observer)
        {
            var type = typeof(T);
            if (!s_Observers.ContainsKey(type))
                s_Observers[type] = new List<object>();

            if (!s_Observers[type].Contains(observer))
                s_Observers[type].Add(observer);
        }

        public static void Unsubscribe<T>(IMyObserver<T> observer)
        {
            var type = typeof(T);
            if (s_Observers.TryGetValue(type, out var list)) return;
            list.Remove(observer);

            if (list.Count == 0)
                s_Observers.Remove(type);
        }

        public static void Notify<T>(T data)
        {
            var type = typeof(T);
            if (!s_Observers.TryGetValue(type, out var list)) return;

            for (int i = list.Count - 1; i >= 0; i--)
            {
                ((IMyObserver<T>)list[i]).OnNotify(data);
            }
        }

        public static void Clear() => s_Observers.Clear();

        #endregion
    }
}