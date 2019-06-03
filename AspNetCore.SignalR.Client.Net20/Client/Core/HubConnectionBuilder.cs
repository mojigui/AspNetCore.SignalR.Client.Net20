using AspNetCore.SignalR.Client.Net20.Connection;
using AspNetCore.SignalR.Client.Net20.Protocol;
using System;
using System.ComponentModel;

namespace AspNetCore.SignalR.Client.Net20.Client
{
    /// <summary>
    /// A builder for configuring <see cref="HubConnection"/> instances.
    /// </summary>
    public class HubConnectionBuilder
    {
        /// <summary>
        /// 是否以构建标识
        /// </summary>
        private bool _hubConnectionBuilt;

        /// <summary>
        /// HttpConnection 的操作项
        /// </summary>
        public HttpConnectionOptions Options { get; set; }

        public HubConnection Build()
        {
            // Build can only be used once
            if (_hubConnectionBuilt)
            {
                throw new InvalidOperationException("同一个 HubConnectionBuilder 只能构建一个连接");
            }

            if (Options == null)
            {
                throw new InvalidOperationException(
                    $"未配置 {nameof(HttpConnectionOptions)} 无法创建 {nameof(HubConnection)} 对象");
            }

            _hubConnectionBuilt = true;

            return new HubConnection(new HttpConnectionFactory(Options), new NewtonsoftJsonHubProtocol());
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Type GetType()
        {
            return base.GetType();
        }
    }
}
