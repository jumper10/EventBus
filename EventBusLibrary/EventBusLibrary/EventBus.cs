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
        private ConcurrentDictionary<string, object> eventTopicMap = new ConcurrentDictionary<string, object>();

        public void Publish<T>(T eventArg, string topic = null)
        {
            GetHandlers<T>(topic, string.IsNullOrEmpty(topic) ? EventBusType.ArgType : EventBusType.Topic)
                  ?.Invoke(eventArg);
        }

        public void Subscribe<T>(Action<T> eventHandler, string topic = null)
        {
            ModfyHandler(eventHandler, topic, string.IsNullOrEmpty(topic) ? EventBusType.ArgType : EventBusType.Topic);
        }

        public void Unsubcribe<T>(Action<T> eventHandler, string topic = null)
        {
            ModfyHandler(eventHandler, topic, string.IsNullOrEmpty(topic) ? EventBusType.ArgType : EventBusType.Topic, false);
        }

        private void ModfyHandler<T>(Action<T> eventHandler, string topic = null, EventBusType eventBusType = EventBusType.ArgType, bool isAdd = true)
        {
            Action<T> handlers = GetHandlers<T>(topic, eventBusType);
            if (isAdd)
                handlers += eventHandler;
            else
                handlers -= eventHandler;
            SetHandlers<T>(handlers, topic, eventBusType);
        }

        private Action<T> GetHandlers<T>(string topic = null, EventBusType eventBusType = EventBusType.ArgType)
        {
            if (eventBusType == EventBusType.ArgType)
            {
                if (!eventMap.ContainsKey(typeof(T)))
                {
                    eventMap[typeof(T)] = null;
                }
                return (Action<T>)eventMap[typeof(T)];
            }
            else
            {
                if (!eventTopicMap.ContainsKey(topic))
                {
                    eventTopicMap[topic] = null;
                }
                return (Action<T>)eventTopicMap[topic];
            }
        }

        private void SetHandlers<T>(Action<T> handlers, string topic = null, EventBusType eventBusType = EventBusType.ArgType)
        {
            if (eventBusType == EventBusType.ArgType)
            {
                eventMap[typeof(T)] = handlers;
            }
            else if (eventBusType == EventBusType.Topic)
            {
                eventTopicMap[topic] = handlers;
            }
        }
        internal enum EventBusType
        {
            ArgType,
            Topic
        }
    }
}
