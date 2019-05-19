using System;

namespace EventBusLibrary
{
    interface IEventBus
    {
        void Subscribe<T>(Action<T> eventHandler, string topic = null);
        void Unsubcribe<T>(Action<T> eventHandler, string topic = null);
        void Publish<T>(T eventArg, string topic = null);
    }
}
