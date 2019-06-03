using AspNetCore.SignalR.Client.Net20.Protocol.Messages;

namespace AspNetCore.SignalR.Client.Net20.Protocol.Handlers
{
    internal interface IMessageHandler
    {
        void Handler(HubMessage hubMessage);
    }
}
