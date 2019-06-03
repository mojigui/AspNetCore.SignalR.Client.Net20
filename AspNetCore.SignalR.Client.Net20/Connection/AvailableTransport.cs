using System.Collections.Generic;

namespace AspNetCore.SignalR.Client.Net20.Connection
{
    public class AvailableTransport
    {
        public string Transport { get; set; }
        public IList<string> TransferFormats { get; set; }
    }
}
