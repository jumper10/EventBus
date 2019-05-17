using System;

namespace EventBusLibrary
{
    interface IEventBus
    {
        void Subscribe<T>(Action<T> eventHandler);
        void Unsubcribe<T>(Action<T> eventHandler);
        void Publish<T>(T eventArg);
    }
}
