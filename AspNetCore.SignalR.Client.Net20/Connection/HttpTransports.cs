namespace AspNetCore.SignalR.Client.Net20.Connection
{
    public static class HttpTransports
    {
        /// <summary>
        /// A bitmask combining all available <see cref="HttpTransportType"/> values.
        /// </summary>
        public static readonly HttpTransportType All = HttpTransportType.WebSockets 
            | HttpTransportType.ServerSentEvents 
            | HttpTransportType.LongPolling;
    }
}
