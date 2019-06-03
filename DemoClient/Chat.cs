using AspNetCore.SignalR.Client.Net20.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DemoClient
{
    public partial class Chat : Form
    {
        private readonly HubConnection _hubConnection;

        public Chat()
        {
            InitializeComponent();

            HubConnectionBuilder builder = new HubConnectionBuilder()
                .WithUrl("http://192.168.1.19:5000/chatHub");
            builder.Options.SkipNegotiation = true;
            _hubConnection = builder.Build();

            _hubConnection.Closed += new EventHandler((s, m) =>
            {
                Invoke(new Action(() =>
                {
                    AddMessages("Closed");
                    Connect.Text = "Connect";
                    SendButton.Enabled = false;
                }));
                Thread.Sleep(new Random().Next(0, 5) * 1000);
                _hubConnection.StartAsync();
            });

            _hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var newMessage = $"{user}: {message}";
                Invoke(new Action(() =>
                {
                    AddMessages(newMessage);
                }));
            });

            SendButton.Enabled = false;
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            try
            {
                if ("Connect".Equals(Connect.Text))
                {
                    Console.WriteLine($"{_hubConnection.ConnectionId}");
                    _hubConnection.StartAsync();
                    AddMessages("Connection started");
                    Connect.Text = "Close Connect";
                    SendButton.Enabled = true;
                }
                else
                {
                    _hubConnection.Stop();
                    AddMessages("Connection closed");
                    Connect.Text = "Connect";
                    SendButton.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                AddMessages(ex.Message);
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            try
            {
                var arg = new UserMessage { User = UserTextBox.Text, Message = MessageTextBox.Text };
                string[] arguments = { UserTextBox.Text, MessageTextBox.Text };
                _hubConnection.InvokeAsync<CallBackMessage>("SendUserMessage", arg, (info, ex) =>
                {
                    AddMessages($"SendUserMessage: {info.Msg}");
                });
            }
            catch (Exception ex)
            {
                AddMessages(ex.Message);
            }
        }

        private void AddMessages(string message)
        {
            if (MessagesList.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    AddMessages(message);
                }));
                return;
            }
            MessagesList.AppendText($"{message ?? string.Empty}{Environment.NewLine}");
        }
    }
}
