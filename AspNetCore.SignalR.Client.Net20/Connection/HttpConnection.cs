using AspNetCore.SignalR.Client.Net20.Client;
using AspNetCore.SignalR.Client.Net20.Client.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace AspNetCore.SignalR.Client.Net20.Connection
{
    public class HttpConnection
    {
        /// <summary>
        /// Negotiation 最大重定向请求次数
        /// </summary>
        private static readonly int _maxRedirects = 100;

        /// <summary>
        /// HttpClient 的超时时间
        /// </summary>
        private static readonly TimeSpan HttpClientTimeout = TimeSpan.FromSeconds(120);

        /// <summary>
        /// 连接锁
        /// </summary>
        private readonly Semaphore _connectionLock = new Semaphore(1, 1);

        /// <summary>
        /// Http 连接的操作项
        /// </summary>
        private readonly HttpConnectionOptions _httpConnectionOptions;

        /// <summary>
        /// 传输协议是否开启标识
        /// </summary>
        private bool _started;

        /// <summary>
        /// 当前连接是否销毁标识
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// 从服务器获取的当前连接的 Connection Id
        /// </summary>
        public string ConnectionId { get; private set; }

        /// <summary>
        /// 当前连接的传输协议
        /// </summary>
        public WebSocketsTransport Transport { get; private set; }

        /// <summary>
        /// 当前连接服务器的 Http 协议 Url
        /// </summary>
        public string Url
        {
            get => _httpConnectionOptions?.Url.AbsoluteUri;
            set => Url = value;
        }

        public HttpConnection(HttpConnectionOptions httpConnectionOptions)
        {
            if (httpConnectionOptions.Url == null)
            {
                throw new ArgumentException($"创建 {nameof(HttpConnection)} 时参数 httpConnectionOptions 的 Url 属性为获取信息"
                    , nameof(httpConnectionOptions));
            }

            Log.Debug($"创建 {nameof(HttpConnection)} 对象");

            _httpConnectionOptions = httpConnectionOptions;
        }

        /// <summary>
        /// 异步方式开启连接
        /// </summary>
        /// <param name="transferFormat">传输格式</param>
        public void StartAsync(TransferFormat transferFormat)
        {
            _ = new Client.Action<TransferFormat>(StartCore).BeginInvoke(transferFormat, null, null);
        }

        /// <summary>
        /// 开启连接
        /// </summary>
        /// <param name="transferFormat">传输格式</param>
        public void Start(TransferFormat transferFormat)
        {
            StartCore(transferFormat);
        }

        /// <summary>
        /// 检测连接是否可用
        /// </summary>
        public void CheckConnectionActive()
        {
            CheckDisposed();
            if (Transport == null || !Transport.IsCanTransport())
            {
                Log.Debug($"当前连接的传输协议未开启");
                throw new Exception($"当前连接的传输协议未开启");
            }
        }

        /// <summary>
        /// 开启连接
        /// </summary>
        /// <param name="transferFormat">传输格式</param>
        private void StartCore(TransferFormat transferFormat)
        {
            CheckDisposed();

            if (_started)
            {
                Log.Debug($"{nameof(HttpConnection)} 连接已开启");
                return;
            }

            if (_connectionLock.WaitOne())
            {
                try
                {
                    CheckDisposed();

                    if (_started)
                    {
                        Log.Debug($"{nameof(HttpConnection)} 连接已开启");
                        return;
                    }

                    Log.Debug($"{nameof(HttpConnection)} 开始连接");

                    SelectAndStartTransport(transferFormat);

                    _started = true;
                    Log.Debug($"{nameof(HttpConnection)} 连接完成");
                }
                finally
                {
                    _connectionLock.Release();
                }
            }
        }

        /// <summary>
        /// 异步方式销毁当前连接
        /// </summary>
        public void DisposeAsync()
        {
            _ = new Action(DisposeCore).BeginInvoke(null, null);
        }

        /// <summary>
        /// 销毁当前连接
        /// </summary>
        public void Dispose()
        {
            DisposeCore();
        }

        private void DisposeCore()
        {
            if (_disposed)
            {
                return;
            }

            _connectionLock.WaitOne();
            try
            {
                if (!_disposed && _started)
                {
                    Log.Debug("Disposing HttpConnection");

                    try
                    {
                        Transport.Stop();
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Stop Transport Error", ex);
                    }

                    Log.Debug("The transport is Stopped");
                }
                else
                {
                    Log.Debug("Skipping Dispose");
                }
            }
            finally
            {
                // We want to do these things even if the WaitForWriterToComplete/WaitForReaderToComplete fails
                if (!_disposed)
                {
                    _disposed = true;
                }

                _connectionLock.Release();
            }
        }

        /// <summary>
        /// 选择传输协议并开启
        /// </summary>
        /// <param name="transferFormat">传输格式</param>
        private void SelectAndStartTransport(TransferFormat transferFormat)
        {
            var uri = _httpConnectionOptions.Url;

            var transportExceptions = new List<Exception>();

            if (_httpConnectionOptions.SkipNegotiation)
            {
                if (_httpConnectionOptions.Transports == HttpTransportType.WebSockets)
                {
                    Log.Debug($"正在开启 {_httpConnectionOptions.Transports.ToString()} 传输");
                    StartTransport(uri);
                }
                else
                {
                    throw new InvalidOperationException("Negotiation 只允许在传输协议为 WebSocket 的情况下跳过");
                }
            }
            else
            {
                NegotiationResponse negotiationResponse;
                var redirects = 0;

                do
                {
                    negotiationResponse = GetNegotiationResponse(uri);

                    if (negotiationResponse.Url != null)
                    {
                        uri = new Uri(negotiationResponse.Url);
                    }

                    redirects++;
                }
                while (negotiationResponse.Url != null && redirects < _maxRedirects);

                if (redirects == _maxRedirects && negotiationResponse.Url != null)
                {
                    throw new InvalidOperationException("Negotiate 重定向超过允许次数");
                }

                // This should only need to happen once
                var connectUrl = CreateConnectUrl(uri, negotiationResponse.ConnectionId);

                var transportType = HttpTransportType.WebSockets;
                var transferFormatString = transferFormat.ToString();

                foreach (var transport in negotiationResponse.AvailableTransports)
                {
                    if (!transportType.ToString().Equals(transport.Transport))
                    {
                        Log.Debug($"客户端不支持 {transport.Transport} 传输协议");
                        transportExceptions.Add(new Exception($"客户端不支持 {transport.Transport} 传输协议"));
                        continue;
                    }

                    try
                    {
                        if (!transport.TransferFormats.Contains(transferFormatString))
                        {
                            Log.Debug($"服务器端使用 {transportType} 不支持 {transferFormatString} 进行传输");
                            transportExceptions.Add(new Exception($"服务器端使用 {transportType} 不支持 {transferFormatString} 进行传输"));
                        }
                        else
                        {
                            // The negotiation response gets cleared in the fallback scenario.
                            if (negotiationResponse == null)
                            {
                                negotiationResponse = GetNegotiationResponse(uri);
                                connectUrl = CreateConnectUrl(uri, negotiationResponse.ConnectionId);
                            }

                            Log.Debug($"正在开启 {_httpConnectionOptions.Transports.ToString()} 传输");
                            StartTransport(connectUrl);
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("验证传输协议或开启传输出错", ex);
                        transportExceptions.Add(ex);
                        negotiationResponse = null;
                    }
                }
            }

            if (Transport == null)
            {
                if (transportExceptions.Count > 0)
                {
                    throw new Exception("使用指定协议和数据格式无法连接到服务器");
                }
                else
                {
                    throw new Exception("客户端不被服务器所支持");
                }
            }
        }

        /// <summary>
        /// 开启指定 Url 的传输协议
        /// </summary>
        /// <param name="url">指定Url</param>
        private void StartTransport(Uri url)
        {
            Transport = new WebSocketsTransport(url, _httpConnectionOptions);
            try
            {
                Transport.Start();
            }
            catch (Exception ex)
            {
                Log.Error($"StartTransport:{nameof(WebSocketsTransport)} 连接出错", ex);

                Transport = null;
                throw;
            }

            Log.Debug($"StartTransport:{nameof(WebSocketsTransport)} 已连接");
        }

        private NegotiationResponse GetNegotiationResponse(Uri uri)
        {
            var negotiationResponse = Negotiate(uri);
            ConnectionId = negotiationResponse.ConnectionId;
            Log.Debug($"Negotiation ConnectionId={ConnectionId}");
            return negotiationResponse;
        }

        private NegotiationResponse Negotiate(Uri url)
        {
            NegotiationResponse negotiateResponse = null;
            try
            {
                var urlBuilder = new UriBuilder(url);
                if (!urlBuilder.Path.EndsWith("/"))
                {
                    urlBuilder.Path += "/";
                }
                urlBuilder.Path += "negotiate";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlBuilder.Uri.AbsoluteUri);
                request.Method = "POST";
                request.ContentType = "text/plain;charset=UTF-8";
                if (int.TryParse($"{HttpClientTimeout.TotalMilliseconds}", out int timeout))
                {
                    request.Timeout = timeout;
                }
                byte[] btBodys = Encoding.UTF8.GetBytes("");
                request.ContentLength = btBodys.Length;
                request.GetRequestStream().Write(btBodys, 0, btBodys.Length);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response != null && HttpStatusCode.OK == response.StatusCode)
                    {
                        StreamReader streamReader = new StreamReader(response.GetResponseStream());
                        string responseContent = streamReader.ReadToEnd();
                        negotiateResponse = JsonConvert.DeserializeObject<NegotiationResponse>(responseContent);
                        if (!string.IsNullOrEmpty(negotiateResponse.Error))
                        {
                            throw new Exception(negotiateResponse.Error);
                        }
                        Log.Debug("Negotiate 请求成功");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Negotiate 请求出错", ex);
                throw ex;
            }

            return negotiateResponse;
        }

        private static Uri CreateConnectUrl(Uri url, string connectionId)
        {
            if (string.IsNullOrEmpty(connectionId))
            {
                throw new FormatException("Invalid connection id.");
            }

            var qs = "id=" + connectionId;
            var builder = new UriBuilder(url);
            var newQueryString = builder.Query;
            if (!string.IsNullOrEmpty(builder.Query))
            {
                newQueryString += "&";
            }
            newQueryString += qs;
            if (newQueryString.Length > 0 && newQueryString[0] == '?')
            {
                newQueryString = newQueryString.Substring(1);
            }
            builder.Query = newQueryString;

            return builder.Uri;
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(HttpConnection));
            }
        }
    }
}
