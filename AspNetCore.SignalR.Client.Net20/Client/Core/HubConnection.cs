using AspNetCore.SignalR.Client.Net20.Connection;
using AspNetCore.SignalR.Client.Net20.Protocol;
using AspNetCore.SignalR.Client.Net20.Protocol.Handlers;
using AspNetCore.SignalR.Client.Net20.Protocol.Messages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace AspNetCore.SignalR.Client.Net20.Client
{
    public class HubConnection
    {
        #region 属性定义
        /// <summary>
        /// 默认的连接超时
        /// 正好是两次默认 ping 的时长
        /// </summary>
        public static readonly TimeSpan DefaultServerTimeout = TimeSpan.FromSeconds(30);
        /// <summary>
        /// 默认的握手超时时间
        /// </summary>
        public static readonly TimeSpan DefaultHandshakeTimeout = TimeSpan.FromSeconds(15);
        /// <summary>
        /// 默认的定时发送 ping 的时长，防止长连接关闭
        /// </summary>
        public static readonly TimeSpan DefaultKeepAliveInterval = TimeSpan.FromSeconds(15);

        /// <summary>
        /// HubConnection 连接锁
        /// </summary>
        private readonly Semaphore _connectionLock = new Semaphore(1, 1);
        /// <summary>
        /// 传输消息的协议
        /// 目前只实现了 NewtonsoftJson
        /// </summary>
        private readonly IHubProtocol _protocol;
        /// <summary>
        /// HttpConnection 连接工厂
        /// </summary>
        private readonly HttpConnectionFactory _connectionFactory;
        /// <summary>
        /// 绑定给远程调用的方法集合
        /// key：方法名
        /// value：方法对应实现列表
        /// </summary>
        private readonly Dictionary<string, InvocationHandlerList> _handlers = new Dictionary<string, InvocationHandlerList>(StringComparer.Ordinal);
        /// <summary>
        /// Invoke 的回调集合
        /// key：InvocationId
        /// value：回调实现
        /// </summary>
        private readonly Dictionary<string, InvocationRequestCallBack<object>> _sendedMessageCallBacks = new Dictionary<string, InvocationRequestCallBack<object>>();

        /// <summary>
        /// Invoke 一次请求的唯一 InvocationId
        /// </summary>
        private long _nextInvocationId = 0;
        /// <summary>
        /// 定时清理 Invoke 的回调集合超时回调的定时器
        /// </summary>
        private Timer _sendedMessageCallBacksCleanerTimer;
        /// <summary>
        /// 定时发送 ping 的定时器
        /// </summary>
        private Timer _sendedPingMessageTimer;
        /// <summary>
        /// 下一次检测连接超时的时间 Tick
        /// </summary>
        private long _nextActivationServerTimeout;
        /// <summary>
        /// 下一次要发送 ping 的时间 Tick
        /// </summary>
        private long _nextActivationSendPing;
        /// <summary>
        /// 当前开启的 HttpConnection
        /// </summary>
        private HttpConnection _connection;
        /// <summary>
        /// 当前 HubConnection 的连接状态
        /// </summary>
        private bool _connectionState;
        /// <summary>
        /// 当前 HubConnection 是否已销毁标识
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// 当前 HubConnection 被关闭的事件
        /// </summary>
        private EventHandler _closed;

        /// <summary>
        /// 当前 HubConnection 被关闭时触发的事件
        /// </summary>
        public event EventHandler Closed
        {
            add
            {
                _closed = (EventHandler)Delegate.Combine(_closed, value);
            }
            remove
            {
                _closed = (EventHandler)Delegate.Remove(_closed, value);
            }
        }

        /// <summary>
        /// 连接的服务超时时间
        /// </summary>
        public TimeSpan ServerTimeout { get; set; } = DefaultServerTimeout;

        /// <summary>
        /// 连接检测是否可用的时长
        /// </summary>
        public TimeSpan KeepAliveInterval { get; set; } = DefaultKeepAliveInterval;

        /// <summary>
        /// 连接握手超时时间
        /// </summary>
        public TimeSpan HandshakeTimeout { get; set; } = DefaultHandshakeTimeout;

        /// <summary>
        /// 当前连接从服务器获取的 ConnectionId
        /// </summary>
        public string ConnectionId { get; private set; }

        /// <summary>
        /// 当前 HubConnection 的连接状态
        /// </summary>
        public HubConnectionState State
        {
            get
            {
                if (_connectionState)
                {
                    return HubConnectionState.Connected;
                }

                return HubConnectionState.Disconnected;
            }
        }
        #endregion

        public HubConnection(HttpConnectionFactory connectionFactory, IHubProtocol protocol)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _protocol = protocol ?? throw new ArgumentNullException(nameof(protocol));

            // 初始化定期清理 Invork 的回调集合定时器
            InitRequestedMessageCallBacksCleaner();
        }

        /// <summary>
        /// 异步方式开启和服务器的连接
        /// </summary>
        public void StartAsync()
        {
            CheckDisposed();
            Log.Debug("HubConnection.StartAsync()");
            _ = new Action(StartCore).BeginInvoke(null, null);
        }

        /// <summary>
        /// 开启和服务器的连接
        /// </summary>
        public void Start()
        {
            CheckDisposed();
            Log.Debug("HubConnection.Start()");
            StartCore();
        }

        /// <summary>
        /// 异步方式停止和服务器的连接
        /// </summary>
        public void StopAsync()
        {
            CheckDisposed();
            Log.Debug("HubConnection.StopAsync()");
            _ = new Action<bool>(StopCore).BeginInvoke(false, null, null);
        }

        /// <summary>
        /// 停止和服务器的连接
        /// </summary>
        public void Stop()
        {
            CheckDisposed();
            Log.Debug("HubConnection.Stop()");
            StopCore(false);
        }

        /// <summary>
        /// 调用服务端方法发送数据，服务端确认后触发回调，每一次调用都有唯一标识
        /// </summary>
        /// <typeparam name="TResult">回调时服务端返回的参数类型</typeparam>
        /// <param name="methodName">服务端方法名</param>
        /// <param name="args">服务端方法参数数组</param>
        /// <param name="callBack">定义的回调</param>
        public void InvokeCoreAsync<TResult>(string methodName, object[] args, Action<TResult, Exception> callBack)
        {
            CheckDisposed();
            _connection.CheckConnectionActive();
            WaitConnectionLock();
            try
            {
                CheckDisposed();
                _ = new Action<string, Action<TResult, Exception>, object[]>(InvokeCore)
                    .BeginInvoke(methodName, callBack, args, null, null);
            }
            finally
            {
                ReleaseConnectionLock();
            }
        }

        /// <summary>
        /// 调用服务端方法发送数据，服务端确认但没有回调，每一次调用都有唯一标识
        /// </summary>
        /// <param name="methodName">服务端方法名</param>
        /// <param name="args">服务端方法参数数组</param>
        public void InvokeCoreAsync(string methodName, object[] args)
        {
            CheckDisposed();
            _connection.CheckConnectionActive();
            WaitConnectionLock();
            try
            {
                CheckDisposed();
                _ = new Action<string, Action<object, Exception>, object[]>(InvokeCore)
                    .BeginInvoke(methodName, null, args, null, null);
            }
            finally
            {
                ReleaseConnectionLock();
            }
        }

        /// <summary>
        /// 调用服务端方法发送数据，不需要服务端确认，并且没有唯一标识
        /// </summary>
        /// <param name="methodName">服务端方法名</param>
        /// <param name="args">服务端方法参数数组</param>
        public void SendCoreAsync(string methodName, object[] args)
        {
            CheckDisposed();
            _connection.CheckConnectionActive();
            WaitConnectionLock();
            try
            {
                CheckDisposed();
                _ = new Action<string, string, object[]>(InnerInvokeCore)
                    .BeginInvoke(methodName, null, args, null, null);
            }
            finally
            {
                ReleaseConnectionLock();
            }
        }

        /// <summary>
        /// 绑定方法到当前连接
        /// </summary>
        /// <param name="methodName">方法名</param>
        /// <param name="parameterTypes">参数类型</param>
        /// <param name="returnType">返回值类型</param>
        /// <param name="handler">方法的具体实现</param>
        /// <returns>IDisposable</returns>
        public IDisposable On(string methodName, Type[] parameterTypes, Type returnType, Func<object[], object> handler)
        {
            CheckDisposed();
            Log.Debug($"Registering Handler methodName={methodName}");

            var invocationHandler = new InvocationHandler(parameterTypes, returnType, handler);
            if (_handlers.TryGetValue(methodName, out InvocationHandlerList invocationList))
            {
                lock (invocationList)
                {
                    invocationList.Add(invocationHandler);
                }
            }
            else
            {
                invocationList = new InvocationHandlerList(invocationHandler);
                lock (_handlers)
                {
                    _handlers.Add(methodName, invocationList);
                }
            }

            return new Subscription(invocationHandler, invocationList);
        }

        /// <summary>
        /// 异步方式销毁当前连接
        /// </summary>
        public void DisposeAsync()
        {
            if (!_disposed)
            {
                _ = new Action<bool>(StopCore).BeginInvoke(true, null, null);
            }
        }

        /// <summary>
        /// 销毁当前连接
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                StopCore(disposing: true);
            }
        }

        /// <summary>
        /// 检测当前连接是否已销毁
        /// </summary>
        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(HubConnection));
            }
        }

        #region 具体方法实现
        /// <summary>
        /// 获取当前连接的下一个InvocationId
        /// </summary>
        /// <returns></returns>
        internal string GetNextInvocationId() => Interlocked.Increment(ref _nextInvocationId).ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// 开启与服务器的连接
        /// </summary>
        private void StartCore()
        {
            WaitConnectionLock();
            try
            {
                if (_connectionState)
                {
                    // We're already connected
                    return;
                }

                CheckDisposed();

                // Start the connection
                _connection = _connectionFactory.Connect(_protocol.TransferFormat);
                _connection.Transport.ReceiveMessageLoop = ReceiveLoopAsync;
                ConnectionId = _connection.ConnectionId;

                //等待2秒 ,WebSocket4Net open之后无论Hub是否能连上初始状态都为connecting,等待2秒后 目标无法连接状态则更改为closed 
                Thread.Sleep(2 * 1000); 
                _connection.CheckConnectionActive();

                Log.Debug($"客户端开始监听服务器端返回,hub uri:{_connection.Url}, " 
                    + $"KeepAliveInterval={(KeepAliveInterval == null ? DefaultKeepAliveInterval : KeepAliveInterval).Seconds}");
                _sendedPingMessageTimer = new Timer(new TimerCallback(RunTimerActions), null,
                    (KeepAliveInterval == null ? DefaultKeepAliveInterval : KeepAliveInterval).Seconds * 1000,
                    (KeepAliveInterval == null ? DefaultKeepAliveInterval : KeepAliveInterval).Seconds * 1000);
                Log.Debug("定期长连接维护设置成功");

                try
                {
                    _connectionState = true;
                    Handshake();
                    Log.Debug("Started");
                }
                catch (Exception ex)
                {
                    Log.Error("Error Starting Connection", ex);
                    ReleaseConnectionLock();
                    Close();
                    throw ex;
                }
            }
            finally
            {
                ReleaseConnectionLock();
            }
        }

        /// <summary>
        /// 停止与服务器的连接
        /// </summary>
        /// <param name="disposing">是否正在执行销毁</param>
        private void StopCore(bool disposing)
        {
            // Block a Start from happening until we've finished capturing the connection state.
            WaitConnectionLock();
            try
            {
                if (disposing && _disposed)
                {
                    // DisposeAsync should be idempotent.
                    return;
                }

                CheckDisposed();
                if (disposing)
                {
                    _sendedMessageCallBacksCleanerTimer.Dispose();
                    _disposed = true;
                }
                if (_sendedPingMessageTimer != null)
                {
                    _sendedPingMessageTimer.Dispose();
                }
                if (_connectionState)
                {
                    _connectionState = false;
                    _connectionFactory.Dispose(_connection);
                    _connection = null;
                    ConnectionId = null;
                }
            }
            finally
            {
                ReleaseConnectionLock();
            }
        }

        /// <summary>
        /// 关闭连接，会触发关闭事件
        /// </summary>
        private void Close()
        {
            Stop();
            if (_closed != null)
            {
                _closed.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Timer 定时检测服务连接是否超时
        /// </summary>
        /// <param name="obj"></param>
        private void RunTimerActions(object obj)
        {
            Log.Debug("长连接维护开始");
            if (_nextActivationServerTimeout != 0 && DateTime.UtcNow.Ticks > _nextActivationServerTimeout)
            {
                Log.Debug("服务器响应超时");
                StopAsync();
            }

            if (DateTime.UtcNow.Ticks > _nextActivationSendPing)
            {
                PingServer();
            }
        }

        /// <summary>
        /// 向服务器发送 Ping 确保连接不会中断
        /// </summary>
        private void PingServer()
        {
            WaitConnectionLock();
            try
            {
                if (_disposed || _connection == null || !_connection.Transport.IsCanTransport())
                {
                    ReleaseConnectionLock();
                    Log.Debug("连接已关闭或者连接已被释放");
                    Close();
                    return;
                }
                Log.Debug("开始发送Ping 给服务器");
                SendHubMessage(PingMessage.Instance);

            }
            catch (Exception ex)
            {
                Log.Error("发送 ping 消息出错", ex);
            }
            finally
            {
                ReleaseConnectionLock();
            }
        }

        /// <summary>
        /// 与服务器进行 Handshake
        /// </summary>
        private void Handshake()
        {
            // Send the Handshake request
            Log.Debug("Sending Hub Handshake");

            try
            {
                var handshakeRequest = new HandshakeRequestMessage(_protocol.Name, _protocol.Version);
                SendHubMessage(handshakeRequest);
                bool isSuccessfulHandshake = false;
                double count = HandshakeTimeout.TotalSeconds;
                while (count-- > 0)
                {
                    if (_connection.Transport.SuccessfulHandshake)
                    {
                        if (_connection.Transport.HandshakeMessage.Error != null)
                        {
                            Log.Debug("Handshake Server Error");
                            throw new Exception(
                                $"Unable to complete handshake with the server due to an error: {_connection.Transport.HandshakeMessage.Error}");
                        }
                        isSuccessfulHandshake = true;
                        break;
                    }
                    Thread.Sleep(1000);
                }
                if (!isSuccessfulHandshake)
                {
                    throw new TimeoutException("Handshake Timeout");
                }
            }
            catch (Exception e)
            {
                Log.Error("Error Receiving Handshake Response", e);
                throw e;
            }
            Log.Debug("Handshake Complete");
        }

        /// <summary>
        /// 向服务器发送 HubMessage 消息
        /// </summary>
        /// <param name="message"></param>
        internal void SendHubMessage(HubMessage message)
        {
            _connection.Transport.SendMessage(_protocol.GetMessageBytes(message));
            UpdateNextActivationTime();
        }

        /// <summary>
        /// 更新下一次发送 Ping 和 ServerTimeout 的时间
        /// </summary>
        private void UpdateNextActivationTime()
        {
            _nextActivationSendPing = (DateTime.UtcNow + (KeepAliveInterval == null ? DefaultKeepAliveInterval : KeepAliveInterval)).Ticks;
            _nextActivationServerTimeout = (DateTime.UtcNow + (ServerTimeout == null ? DefaultServerTimeout : ServerTimeout)).AddSeconds(1).Ticks;
        }

        /// <summary>
        /// 调用服务器方法
        /// </summary>
        /// <typeparam name="TResult">回调时服务器返回的数据类型</typeparam>
        /// <param name="methodName">服务器方法名</param>
        /// <param name="callBack">回调实现</param>
        /// <param name="args">参数数组</param>
        private void InvokeCore<TResult>(string methodName, Action<TResult, Exception> callBack, object[] args)
        {
            var currentInvocationId = GetNextInvocationId();
            if (_sendedMessageCallBacks.TryGetValue(currentInvocationId, out InvocationRequestCallBack<object> invocationRequestCallBack))
            {
                throw new Exception($"HubUrl:{_connection.Url},currentInvocationId:{currentInvocationId} 在 InvocationRequestCallBack 中已存在");
            }

            Action<object, Exception> invoke = null;
            if (callBack != null)
            {
                invoke = (obj, ex) =>
                {
                    var result = (TResult)obj;
                    if (result == null)
                    {
                        ex = new Exception("方法Invoke 中的返回值 与服务器实际返回的值类型不匹配");
                    }
                    callBack(result, ex);
                };
            }
            lock(_sendedMessageCallBacks)
            {
                _sendedMessageCallBacks.Add(currentInvocationId, new InvocationRequestCallBack<object>(DateTime.UtcNow.AddMinutes(InvocationRequestCallBack<object>.CallBackTimeOutMinutes), invoke, typeof(TResult)));
            }
            InnerInvokeCore(methodName, currentInvocationId, args);
        }

        private void InnerInvokeCore(string methodName, string invocationId, object[] args)
        {
            InvocationMessage invocationMessage;
            if (string.IsNullOrEmpty(invocationId))
            {
                invocationMessage = new InvocationMessage(methodName, args);
            }
            else
            {
                invocationMessage = new InvocationMessage(invocationId, methodName, args);
            }
            Log.Debug("开始发送远程调用消息");
            SendHubMessage(invocationMessage);
        }

        /// <summary>
        /// 初始化定时清理消息请求回调
        /// </summary>
        private void InitRequestedMessageCallBacksCleaner()
        {
            _sendedMessageCallBacksCleanerTimer = new Timer((state) =>
            {
                // 清理时 停止 消息发送 或等待正在发送的消息发送完成
                WaitConnectionLock();

                try
                {
                    Log.Debug("InvocationRequestCallBack 定期清理启动");

                    var shouldCleanCallBackInvocationIds = new List<string>();
                    foreach (var sendedMessageCallBack in _sendedMessageCallBacks)
                    {
                        if (sendedMessageCallBack.Value.ExpireTime <= DateTime.UtcNow)
                        {
                            shouldCleanCallBackInvocationIds.Add(sendedMessageCallBack.Key);
                        }
                    }
                    if (shouldCleanCallBackInvocationIds.Count > 0)
                    {
                        lock(_sendedMessageCallBacks)
                        {
                            shouldCleanCallBackInvocationIds.ForEach(m =>
                            {
                                _sendedMessageCallBacks.Remove(m);
                                Log.Error($"InvocationId:{m}的 请求回调从回调池里删除,请求未在{InvocationRequestCallBack<object>.CallBackTimeOutMinutes}分钟内响应", null);
                            });
                        }
                    }
                    Log.Debug($"InvocationRequestCallBack 定期清理结束,清理对象:{shouldCleanCallBackInvocationIds.Count}个");
                }
                finally
                {
                    ReleaseConnectionLock();
                }

            }, null, 5 * 1000 * 60, 5 * 1000 * 60);
            Log.Debug("InvocationRequestCallBack 定期清理设置成功");
        }

        /// <summary>
        /// 接收到服务器消息的处理事件具体实现
        /// </summary>
        /// <param name="message">接收的服务器消息</param>
        private void ReceiveLoopAsync(string message)
        {
            try
            {
                if (_protocol.TryParseMessage(message, out HubMessage hubMessage))
                {
                    ProcessMessages(hubMessage);
                }
            }
            catch (Exception e)
            {
                Log.Error("处理接收消息出错", e);
            }
        }

        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="hubMessage">转换后的消息对象</param>
        private void ProcessMessages(HubMessage hubMessage)
        {
            if (hubMessage == null)
            {
                Log.Debug("为获取到可处理的消息");
                throw new ArgumentNullException($"为获取到可处理的消息.{nameof(hubMessage)}");
            }
            switch (hubMessage)
            {
                case InvocationMessage invocation:
                    {
                        var handler = new InvocationMessageHandler(_handlers, (r) =>
                        {
                            if (!string.IsNullOrEmpty(invocation.InvocationId))
                            {
                                var completionMessage = new CompletionMessage(invocation.InvocationId, null, r, r != null);
                                SendHubMessage(completionMessage);
                            }
                        });
                        try
                        {
                            handler.Handler(invocation);
                        }
                        catch (Exception e)
                        {
                            Log.Error(null, e);
                            if (invocation != null && !string.IsNullOrEmpty(invocation.InvocationId))
                            {
                                SendHubMessage(new CompletionMessage(invocation.InvocationId, e.Message, null, false));
                            }
                        }
                    }
                    break;
                case StreamItemMessage streamItem:
                    {
                        Log.Info($"客户端暂不支持 {nameof(StreamItemMessage)}");
                        if (streamItem != null && !string.IsNullOrEmpty(streamItem.InvocationId))
                        {
                            SendHubMessage(new CompletionMessage(streamItem.InvocationId, $"客户端暂不支持 {nameof(StreamItemMessage)}", null, false));
                        }
                    }
                    break;
                case StreamInvocationMessage streamInvocation:
                    {
                        Log.Info($"客户端暂不支持 {nameof(StreamInvocationMessage)}");
                        if (streamInvocation != null && !string.IsNullOrEmpty(streamInvocation.InvocationId))
                        {
                            SendHubMessage(new CompletionMessage(streamInvocation.InvocationId, $"客户端暂不支持 {nameof(StreamInvocationMessage)}", null, false));
                        }
                    }
                    break;
                case CancelInvocationMessage cancelInvocation:
                    {
                        Log.Info($"客户端暂不支持 {nameof(CancelInvocationMessage)}");
                    }
                    break;
                case CompletionMessage completion:
                    {
                        var handler = new CompletionMessageHandler(_sendedMessageCallBacks);
                        handler.Handler(completion);
                        // 清除当前 InvocationId 对应的回调
                        if (_sendedMessageCallBacks.ContainsKey(completion.InvocationId))
                        {
                            lock (_sendedMessageCallBacks)
                            {
                                _sendedMessageCallBacks.Remove(completion.InvocationId);
                            }
                        }
                    }
                    break;
                case CloseMessage close:
                    {
                        if (string.IsNullOrEmpty(close.Error))
                        {
                            Log.Debug("服务器将关闭连接，客户端主动断开连接");
                        }
                        else
                        {
                            Log.Error("服务器将关闭连接，客户端主动断开连接", new Exception(close.Error));
                        }
                        StopAsync();
                    }
                    break;
                case PingMessage _:
                    Log.Debug("接收到服务器端 Ping");
                    break;
                default:
                    throw new InvalidOperationException($"未知的消息类型: {hubMessage.GetType().FullName}");
            }
        }

        private void WaitConnectionLock()
        {
            _connectionLock.WaitOne();
        }

        private void ReleaseConnectionLock()
        {
            _connectionLock.Release();
        }
        #endregion
    }
}
