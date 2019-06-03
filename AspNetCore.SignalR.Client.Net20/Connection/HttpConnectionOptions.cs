using AspNetCore.SignalR.Client.Net20.Protocol;
using System;
using System.Collections.Generic;

namespace AspNetCore.SignalR.Client.Net20.Connection
{
    public class HttpConnectionOptions
    {
        private IDictionary<string, string> _headers;

        public HttpConnectionOptions(Uri uri) : this()
        {
            Url = uri;
        }

        public HttpConnectionOptions()
        {
            _headers = new Dictionary<string, string>();

            Transports = HttpTransports.All;
        }

        public IDictionary<string, string> Headers
        {
            get => _headers;
            set => _headers = value ?? throw new ArgumentNullException(nameof(value));
        }

        public Uri Url { get; set; }

        public HttpTransportType Transports { get; set; }

        public ProtocolOption ProtocolOption { get; private set; } = ProtocolOption.Json;

        /// <summary>
        /// Gets or sets a value indicating whether negotiation is skipped when connecting to the server.
        /// </summary>
        /// <remarks>
        /// Negotiation can only be skipped when using the <see cref="HttpTransportType.WebSockets"/> transport.
        /// </remarks>
        public bool SkipNegotiation { get; set; }
    }
}
