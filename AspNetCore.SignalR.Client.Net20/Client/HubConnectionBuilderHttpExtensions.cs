using AspNetCore.SignalR.Client.Net20.Client.Core;
using AspNetCore.SignalR.Client.Net20.Connection;
using System;

namespace AspNetCore.SignalR.Client.Net20.Client
{
    /// <summary>
    /// Extension methods for <see cref="HubConnectionBuilder"/>.
    /// </summary>
    public static class HubConnectionBuilderHttpExtensions
    {
        /// <summary>
        /// Configures the <see cref="HubConnection" /> to use HTTP-based transports to connect to the specified URL.
        /// </summary>
        /// <param name="hubConnectionBuilder">The <see cref="HubConnectionBuilder" /> to configure.</param>
        /// <param name="url">The URL the <see cref="HttpConnection"/> will use.</param>
        /// <returns>The same instance of the <see cref="HubConnectionBuilder"/> for chaining.</returns>
        public static HubConnectionBuilder WithUrl(this HubConnectionBuilder hubConnectionBuilder, string url)
        {
            hubConnectionBuilder.WithUrlCore(new Uri(url));
            return hubConnectionBuilder;
        }

        /// <summary>
        /// Configures the <see cref="HubConnection" /> to use HTTP-based transports to connect to the specified URL and transports.
        /// </summary>
        /// <param name="hubConnectionBuilder">The <see cref="HubConnectionBuilder" /> to configure.</param>
        /// <param name="url">The URL the <see cref="HttpConnection"/> will use.</param>
        /// <param name="transports">A bitmask combining one or more <see cref="HttpTransportType"/> values that specify what transports the client should use.</param>
        /// <returns>The same instance of the <see cref="HubConnectionBuilder"/> for chaining.</returns>
        public static HubConnectionBuilder WithUrl(this HubConnectionBuilder hubConnectionBuilder, string url, HttpTransportType transports)
        {
            hubConnectionBuilder.WithUrlCore(new Uri(url), transports);
            return hubConnectionBuilder;
        }

        /// <summary>
        /// Configures the <see cref="HubConnection" /> to use HTTP-based transports to connect to the specified URL.
        /// </summary>
        /// <param name="hubConnectionBuilder">The <see cref="HubConnectionBuilder" /> to configure.</param>
        /// <param name="url">The URL the <see cref="HttpConnection"/> will use.</param>
        /// <returns>The same instance of the <see cref="HubConnectionBuilder"/> for chaining.</returns>
        public static HubConnectionBuilder WithUrl(this HubConnectionBuilder hubConnectionBuilder, Uri url)
        {
            hubConnectionBuilder.WithUrlCore(url);
            return hubConnectionBuilder;
        }

        /// <summary>
        /// Configures the <see cref="HubConnection" /> to use HTTP-based transports to connect to the specified URL and transports.
        /// </summary>
        /// <param name="hubConnectionBuilder">The <see cref="HubConnectionBuilder" /> to configure.</param>
        /// <param name="url">The URL the <see cref="HttpConnection"/> will use.</param>
        /// <param name="transports">A bitmask combining one or more <see cref="HttpTransportType"/> values that specify what transports the client should use.</param>
        /// <returns>The same instance of the <see cref="HubConnectionBuilder"/> for chaining.</returns>
        public static HubConnectionBuilder WithUrl(this HubConnectionBuilder hubConnectionBuilder, Uri url, HttpTransportType transports)
        {
            hubConnectionBuilder.WithUrlCore(url, transports);
            return hubConnectionBuilder;
        }

        private static HubConnectionBuilder WithUrlCore(this HubConnectionBuilder hubConnectionBuilder, Uri url, HttpTransportType transports = HttpTransportType.WebSockets)
        {
            if (hubConnectionBuilder == null)
            {
                throw new ArgumentNullException(nameof(hubConnectionBuilder));
            }
            hubConnectionBuilder.Options = new HttpConnectionOptions
            {
                Url = url,
                Transports = transports
            };
            return hubConnectionBuilder;
        }
    }
}
