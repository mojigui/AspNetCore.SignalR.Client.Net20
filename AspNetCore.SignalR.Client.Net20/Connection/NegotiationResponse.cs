using System.Collections.Generic;

namespace AspNetCore.SignalR.Client.Net20.Connection
{
    public class NegotiationResponse
    {
        public string Url { get; set; }
        public string AccessToken { get; set; }
        public string ConnectionId { get; set; }
        public IList<AvailableTransport> AvailableTransports { get; set; }
        public string Error { get; set; }
    }
}
