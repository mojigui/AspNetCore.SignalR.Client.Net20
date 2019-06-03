using AspNetCore.SignalR.Client.Net20.Connection;
using AspNetCore.SignalR.Client.Net20.Protocol;
using AspNetCore.SignalR.Client.Net20.Protocol.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocket4Net;

namespace AspNetCore.SignalR.Client.Net20.Client.Core
{
    public class WebSocketsTransport
    {
        /// <summary>
        /// WebSocket 客户端
        /// </summary>        
        private readonly WebSocket _webSocket;

        /// <summary>
        /// WebSocket 协议的服务端连接URL
        /// </summary>
        private readonly Uri _url;

        /// <summary>
        /// 自定义的请求头列表
        /// </summary>
        private readonly List<KeyValuePair<string, string>> _customHeaderItems;

        /// <summary>
        /// 与服务器握手是否成功
        /// </summary>
        public bool SuccessfulHandshake { get; private set; }

        /// <summary>
        /// 握手成功后服务器返回的消息
        /// </summary>
        public HandshakeResponseMessage HandshakeMessage { get; private set; }

        /// <summary>
        /// 接收到服务器消息的处理事件
        /// </summary>
        public Action<string> ReceiveMessageLoop { get; set; }

        public WebSocketsTransport(Uri url, HttpConnectionOptions httpConnectionOptions)
        {
            if (httpConnectionOptions == null)
            {
                throw new ArgumentNullException(nameof(httpConnectionOptions));
            }

            if (httpConnectionOptions.Url == null)
            {
                throw new ArgumentNullException(nameof(httpConnectionOptions.Url));
            }
            _url = ResolveWebSocketsUrl(url ?? httpConnectionOptions.Url);

            _customHeaderItems = new List<KeyValuePair<string, string>>();
            if (httpConnectionOptions.Headers != null)
            {
                foreach (var header in httpConnectionOptions.Headers)
                {
                    _customHeaderItems.Add(new KeyValuePair<string, string>(header.Key, header.Value));
                }
            }
            _customHeaderItems.Add(new KeyValuePair<string, string>("X-Requested-With", "XMLHttpRequest"));
            _webSocket = new WebSocket(_url.AbsoluteUri, string.Empty, null, _customHeaderItems, null, WebSocketVersion.None);
        }

        /// <summary>
        /// 开启 WebSocket 通讯
        /// </summary>
        public void Start()
        {
            Log.Debug($"StartTransport url=[{_url.AbsoluteUri}]");
            _webSocket.Opened += WebSocket_Opened;
            _webSocket.Closed += WebSocket_Closed;
            _webSocket.MessageReceived += WebSocket_MessageReceived;
            try
            {
                _webSocket.Open();
            }
            catch
            {
                _webSocket.Dispose();
                throw;
            }
        }

        /// <summary>
        /// 开启 WebSocket 通讯，并销毁
        /// </summary>
        public void Stop()
        {
            Log.Debug("Transport Stopping");

            _webSocket.Dispose();

            Log.Debug("Transport Stopped");
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">字符串格式的消息</param>
        public void SendMessage(string message)
        {
            Log.Debug($"send message[JSON]: {message}");
            try
            {
                if (IsCanTransport())
                {
                    _webSocket.Send(message);
                }
            } 
            catch (Exception e)
            {
                Log.Error("send message failed[JSON]", e);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="data">字节数组格式的消息</param>
        public void SendMessage(byte[] data)
        {
            Log.Debug($"send message[BYTE]: {data}");
            try
            {
                if (IsCanTransport())
                {
                    _webSocket.Send(data, 0, data.Length);
                }
            }
            catch (Exception e)
            {
                Log.Error("send message failed[BYTE]", e);
            }
        }

        /// <summary>
        /// 检测是否可以传输消息
        /// </summary>
        /// <returns>bool</returns>
        public bool IsCanTransport()
        {
            return _webSocket != null && _webSocket.State == WebSocketState.Open;
        }

        /// <summary>
        /// WebSocket 接收到消息的处理时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebSocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var data = e.Message;
            Log.Debug($"收到服务端消息:{data}");
            if (data.Length < 1)
            {
                return; //非 signalR 协议中的返回
            }
            data = data.Substring(0, data.Length - 1);
            var separator = new byte[1] { 0x1e };
            var separatorStr = Encoding.UTF8.GetString(separator);
            var messages = data.Split(separatorStr[0]);
            if (messages == null || messages.Length <= 0)
            {
                Log.Debug("收到的服务端消息未解析到有用信息");
                return;
            }
            foreach (var message in messages)
            {
                try
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        bool isHandshakeResponse = HandshakeProtocol
                            .TryParseHandshakeResponseMessage(message, out HandshakeResponseMessage handshakeResponseMessage);
                        Log.Debug($"SuccessfulHandshake={SuccessfulHandshake}, TryParseHandshakeResponseMessage={isHandshakeResponse}");
                        if (SuccessfulHandshake && !isHandshakeResponse)
                        {
                            if (ReceiveMessageLoop != null)
                            {
                                ReceiveMessageLoop.BeginInvoke(message, null, null);
                            }
                        }
                        else if (!SuccessfulHandshake && isHandshakeResponse)
                        {
                            SuccessfulHandshake = true;
                            HandshakeMessage = handshakeResponseMessage;
                        }
                    }
                }
                catch (Exception e1)
                {
                    Log.Error("WebSocket 处理消息出错", e1);
                    continue;
                }
            }
        }

        private void WebSocket_Opened(object sender, EventArgs e)
        {
            Log.Debug("WebSocket Opened");
        }

        private void WebSocket_Closed(object sender, EventArgs e)
        {
            Log.Debug("WebSocket Closed");
        }

        /// <summary>
        /// 将 Http 协议的 Url 转换为 WebSocket 协议的
        /// </summary>
        /// <param name="url">Http协议的Url</param>
        /// <returns>WebSocket协议的Url</returns>
        private static Uri ResolveWebSocketsUrl(Uri url)
        {
            var uriBuilder = new UriBuilder(url);
            if (url.Scheme == "http")
            {
                uriBuilder.Scheme = "ws";
            }
            else if (url.Scheme == "https")
            {
                uriBuilder.Scheme = "wss";
            }

            return uriBuilder.Uri;
        }
    }
}
