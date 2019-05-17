using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusLibrary
{
    interface IEventBus
    {
        void Subscribe<T>(Action<T> eventHandler);
        void Unsubcribe<T>(Action<T> eventHandler);
        void Publish<T>(T eventArg);
    }
}
