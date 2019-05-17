using System;
using System.Collections.Concurrent;

namespace EventBusLibrary
{
    public class EventBus : IEventBus
    {
        private static EventBus _default;

        public static EventBus Default
        {
            get
            {
                if (_default == null)
                    _default = new EventBus();
                return _default;
            }
        }
        private ConcurrentDictionary<Type, object> eventMap = new ConcurrentDictionary<Type, object>();

        public void Publish<T>(T eventArg)
        {
            object handler;
            if (eventMap.TryGetValue(typeof(T), out handler))
            {
                ((Action<T>)handler)?.Invoke(eventArg);
            }
        }

        public void Subscribe<T>(Action<T> eventHandler)
        {
            if (eventMap.ContainsKey(typeof(T)))
            {
                AddHandler(eventHandler);
            }
            else
            {
                eventMap[typeof(T)] = null;
                AddHandler(eventHandler);
            }
        }

        public void Unsubcribe<T>(Action<T> eventHandler)
        {

            if (eventMap.ContainsKey(typeof(T)))
            {
                RemoveHandler(eventHandler);
            }
        }

        private void AddHandler<T>(Action<T> eventHandler)
        {
            var handlers = (Action<T>)eventMap[typeof(T)];
            handlers += eventHandler;
            eventMap[typeof(T)] = handlers;
        }

        private void RemoveHandler<T>(Action<T> eventHandler)
        {
            Action<T> handlers = (Action<T>)eventMap[typeof(T)];
            handlers -= eventHandler;
            eventMap[typeof(T)] = handlers;
        }
    }
}
