using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusLibrary
{
    public class EventBus : IEventBus
    {

        private static EventBus _default;

        public static EventBus Default
        {
            get {
                if (_default == null)
                    _default = new EventBus();
                return _default;
            }
        }

        private ConcurrentDictionary<Type, object> eventMap = new ConcurrentDictionary<Type, object>();

        private ConcurrentDictionary<string, object> eventKeyMap = new ConcurrentDictionary<string, object>();

        public void Publish<T>(T eventArg)
        {
            object handler;
            if (eventMap.TryGetValue(typeof(T), out handler))
            {
                ((Action<T>)handler).Invoke(eventArg);
            }
        }

        public void Subscribe<T>(Action<T> eventHandler)
        {
            if (eventMap.ContainsKey(typeof(T)))
            {
                Action<T> handler = (Action<T>)eventMap[typeof(T)];
                handler += eventHandler;
            }
            else
            {
                eventMap[typeof(T)] = eventHandler;
            }
        }

        public void Unsubcribe<T>(Action<T> eventHandler)
        {

            if (eventMap.ContainsKey(typeof(T)))
            {
                Action<T> handler = (Action<T>)eventMap[typeof(T)];
                handler -= eventHandler;
            }
        }
    }
}
