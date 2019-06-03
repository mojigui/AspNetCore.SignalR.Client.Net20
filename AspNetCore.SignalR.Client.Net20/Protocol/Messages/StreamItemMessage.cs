namespace AspNetCore.SignalR.Client.Net20.Protocol.Messages
{
    public class StreamItemMessage : HubInvocationMessage
    {
        public object Item { get; }

        public StreamItemMessage()
        {
        }

        public StreamItemMessage(string invocationId, object item) : base(invocationId)
        {
            Type = HubProtocolConstants.StreamItemMessageType;
            Item = item;
        }

        public override string ToString()
        {
            return $"StreamItem {{ {nameof(InvocationId)}: \"{InvocationId}\", {nameof(Item)}: {Item ?? "<<null>>"} }}";
        }
    }
}
