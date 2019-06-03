namespace AspNetCore.SignalR.Client.Net20.Protocol.Messages
{
    public class CancelInvocationMessage : HubInvocationMessage
    {

        public CancelInvocationMessage()
        {
        }

        public CancelInvocationMessage(string invocationId) : base(invocationId)
        {
            Type = HubProtocolConstants.CancelInvocationMessageType;
        }
    }
}
