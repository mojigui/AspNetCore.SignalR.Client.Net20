using System;
using System.Collections.Generic;
using AspNetCore.SignalR.Client.Net20.Client;

namespace AspNetCore.SignalR.Client.Net20.Protocol.Handlers
{
    public class InvocationHandlerList
    {
        private readonly List<InvocationHandler> _invocationHandlers;
        // A lazy cached copy of the handlers that doesn't change for thread safety.
        // Adding or removing a handler sets this to null.
        private InvocationHandler[] _copiedHandlers;

        internal InvocationHandlerList(InvocationHandler handler)
        {
            _invocationHandlers = new List<InvocationHandler>() { handler };
        }

        internal InvocationHandler[] GetHandlers()
        {
            var handlers = _copiedHandlers;
            if (handlers == null)
            {
                lock (_invocationHandlers)
                {
                    // Check if the handlers are set, if not we'll copy them over.
                    if (_copiedHandlers == null)
                    {
                        _copiedHandlers = _invocationHandlers.ToArray();
                    }
                    handlers = _copiedHandlers;
                }
            }
            return handlers;
        }

        internal void Add(InvocationHandler handler)
        {
            lock (_invocationHandlers)
            {
                _invocationHandlers.Add(handler);
                _copiedHandlers = null;
            }
        }

        internal void Remove(InvocationHandler handler)
        {
            lock (_invocationHandlers)
            {
                if (_invocationHandlers.Remove(handler))
                {
                    _copiedHandlers = null;
                }
            }
        }
    }

    public class InvocationHandler
    {
        public Type[] ParameterTypes { get; }

        public Type ReturnType { get; private set; }

        private readonly Client.Func<object[], object> _callback;

        public InvocationHandler(Type[] parameterTypes, Type returnType, Client.Func<object[], object> callback)
        {
            ParameterTypes = parameterTypes;
            ReturnType = returnType;
            _callback = callback;
        }

        internal void InvokeAsync(object[] parameters, Client.Action<object> returnTResultAsync)
        {
            _callback.BeginInvoke(parameters, (ar) =>
            {
                Client.Func<object[], object> method = (Client.Func<object[], object>)ar.AsyncState;
                //委托执行的异常会在EndInvoke时抛出来 
                try
                {
                    var result = method.EndInvoke(ar);
                    Log.Debug($"执行 EndInvoke 后 IAsyncResult.Completed={ar.IsCompleted}");
                    // 方法执行完成后执行指定的异步方法
                    if (!ar.IsCompleted)
                    {
                        throw new OverflowException($"IAsyncResult Is Not Completed");
                    }
                    returnTResultAsync?.BeginInvoke(result, null, null);
                }
                catch (OverflowException oe)
                {
                    Log.Error($"{nameof(InvocationHandler)} 执行 EndInvoke 出错", oe);
                    throw oe;
                }
            }, _callback);
        }
    }
}
