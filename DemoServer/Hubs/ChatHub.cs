using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Protocol;
using DemoServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoServer.Hubs
{
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", "Server", "Welcome");
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task<CallBackMessage> SendUserMessage(UserMessage userMessage)
        {
            await Clients.All.SendAsync("ReceiveMessage", userMessage.User, userMessage.Message);
            return new CallBackMessage { Msg = $"{userMessage.User} say {userMessage.Message}" };
        }
    }
}
