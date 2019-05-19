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
                ModfyHandler(eventHandler);
            }
            else
            {
                eventMap[typeof(T)] = null;
                ModfyHandler(eventHandler);
            }
        }

        public void Unsubcribe<T>(Action<T> eventHandler)
        {

            if (eventMap.ContainsKey(typeof(T)))
            {
                ModfyHandler(eventHandler, false);
            }
        }

        private void ModfyHandler<T>(Action<T> eventHandler, bool isAdd = true)
        {
            Action<T> handlers = (Action<T>)eventMap[typeof(T)];
            if (isAdd)
                handlers += eventHandler;
            else
                handlers -= eventHandler;
            eventMap[typeof(T)] = handlers;
        }
    }
}
