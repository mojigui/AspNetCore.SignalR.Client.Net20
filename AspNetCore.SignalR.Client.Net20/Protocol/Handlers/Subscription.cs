using System;

namespace AspNetCore.SignalR.Client.Net20.Protocol.Handlers
{
    internal class Subscription : IDisposable
    {
        private readonly InvocationHandler _handler;
        private readonly InvocationHandlerList _handlerList;

        public Subscription(InvocationHandler handler, InvocationHandlerList handlerList)
        {
            _handler = handler;
            _handlerList = handlerList;
        }

        public void Dispose()
        {
            _handlerList.Remove(_handler);
        }
    }
}
