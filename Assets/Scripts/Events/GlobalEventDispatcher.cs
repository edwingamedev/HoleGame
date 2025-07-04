using System;
using System.Collections.Generic;

namespace EdwinGameDev.EventSystem
{
    public static class GlobalEventDispatcher
    {
        private static readonly Dictionary<Type, List<Delegate>> Subscribers = new();
    
        public static void AddSubscriber<T>(Action<T> callback)
        {
            Type type = typeof(T);
            if (!Subscribers.TryGetValue(type, out var list))
            {
                list = new List<Delegate>();
                Subscribers[type] = list;
            }

            if (list.Contains(callback))
            {
                return;
            }

            list.Add(callback);
        }
    
        public static void RemoveSubscriber<T>(Action<T> callback)
        {
            Type type = typeof(T);
            
            if (!Subscribers.TryGetValue(type, out List<Delegate> list))
            {
                return;
            }

            list.Remove(callback);
        }
    
        public static void Publish<T>(T eventData)
        {
            Type type = typeof(T);
            if (!Subscribers.TryGetValue(type, out List<Delegate> list))
            {
                return;
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] is Action<T> callback)
                {
                    callback.Invoke(eventData);
                }
            }
        }
    }
}