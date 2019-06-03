namespace AspNetCore.SignalR.Client.Net20.Protocol.Messages
{
    public class PingMessage : HubMessage
    {
        public static readonly PingMessage Instance = new PingMessage();

        private PingMessage()
        {
            Type = HubProtocolConstants.PingMessageType;
        }
    }
}
