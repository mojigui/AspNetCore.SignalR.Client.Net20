using AspNetCore.SignalR.Client.Net20.Protocol.Messages;
using System;
using System.Collections.Generic;

namespace AspNetCore.SignalR.Client.Net20.Protocol.Handlers
{
    internal class InvocationMessageHandler : IMessageHandler
    {
        /// <summary>
        /// 给远程调用的方法集合
        /// </summary>
        private readonly Dictionary<string, InvocationHandlerList> _handlers;
        /// <summary>
        /// 方法调用完以后返回结果个服务器的响应事件
        /// </summary>
        private readonly Client.Action<object> _returnHandler;

        public InvocationMessageHandler(Dictionary<string, InvocationHandlerList> handlers, Client.Action<object> returnHandler)
        {
            _handlers = handlers;
            _returnHandler = returnHandler;
        }

        public void Handler(HubMessage hubMessage)
        {
            Log.Debug($"开始处理 {nameof(InvocationMessage)}");
            InvocationMessage message;
            try
            {
                message = hubMessage as InvocationMessage;
            }
            catch (Exception e)
            {
                throw new Exception($"方法调用要处理的消息不是 {nameof(InvocationMessage)} 类型, type={hubMessage?.Type}", e);
            }
            if (message == null)
            {
                throw new Exception($"方法调用要处理的 {nameof(CompletionMessage)} 类型消息为空");
            }
            if (!_handlers.TryGetValue(message.Target, out InvocationHandlerList invocationList))
            {
                throw new Exception($"绑定的方法集合里不存在 methodName={message.Target} 的定义");
            }
            var copiedHandlers = invocationList?.GetHandlers();
            // 回调未定义具体实现
            if (copiedHandlers == null || copiedHandlers.Length <= 0)
            {
                return;
            }
            foreach (var handler in copiedHandlers)
            {
                try
                {
                    handler.InvokeAsync(message.Arguments, _returnHandler);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error Invoking Client Method {message.Target}", ex);
                }
            }
        }
    }
}
