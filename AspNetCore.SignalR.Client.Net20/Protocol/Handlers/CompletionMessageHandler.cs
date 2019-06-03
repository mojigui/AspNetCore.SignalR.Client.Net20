using AspNetCore.SignalR.Client.Net20.Protocol.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AspNetCore.SignalR.Client.Net20.Protocol.Handlers
{
    internal class CompletionMessageHandler : IMessageHandler
    {
        // Invoke 的回调集合
        private readonly Dictionary<string, InvocationRequestCallBack<object>> _callBacks;

        public CompletionMessageHandler(Dictionary<string, InvocationRequestCallBack<object>> callBacks)
        {
            _callBacks = callBacks;
        }

        public void Handler(HubMessage hubMessage)
        {
            Log.Debug($"开始处理 {nameof(CompletionMessage)}");
            CompletionMessage message;
            try
            {
                message = hubMessage as CompletionMessage;
            }
            catch (Exception e)
            {
                Log.Error($"回调要处理的消息不是 {nameof(CompletionMessage)} 类型", e);
                return;
            }
            if (message == null)
            {
                Log.Debug($"回调要处理的 {nameof(CompletionMessage)} 类型消息为空");
            }
            if (!_callBacks.TryGetValue(message.InvocationId, out InvocationRequestCallBack<object> callback))
            {
                Log.Debug($"Invoke 的回调集合里不存在 InvocationId={message.InvocationId} 的定义");
            }
            // 回调未定义具体实现
            if (callback.Invoke == null)
            {
                return;
            }
            try
            {
                if (message.HasResult)
                {
                    var settings = NewtonsoftJsonHubProtocol.CreateDefaultSerializerSettings();
                    var resultJson = JsonConvert.SerializeObject(message.Result, settings);
                    var result = JsonConvert.DeserializeObject(resultJson, callback.ReturnType, settings);
                    if (!string.IsNullOrEmpty(message.Error))
                    {
                        callback.Invoke.BeginInvoke(result, new Exception(message.Error), null, null);
                    } 
                    else
                    {
                        callback.Invoke.BeginInvoke(result, null, null, null);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(message.Error))
                    {
                        callback.Invoke.BeginInvoke(null, new Exception(message.Error), null, null);
                    }
                    else
                    {
                        callback.Invoke.BeginInvoke(null, null, null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("回调失败", ex);
            }
        }
    }
}
