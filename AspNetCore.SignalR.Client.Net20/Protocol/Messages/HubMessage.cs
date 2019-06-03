namespace AspNetCore.SignalR.Client.Net20.Protocol.Messages
{
    /// <summary>
    /// A base class for hub messages.
    /// </summary>
    public abstract class HubMessage
    {
        public int? Type { get; set; }
    }

    public class ReceivedMessage : HubMessage
    {
    }
}
