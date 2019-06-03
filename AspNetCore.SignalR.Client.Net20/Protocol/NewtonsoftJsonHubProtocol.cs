using AspNetCore.SignalR.Client.Net20.Client;
using AspNetCore.SignalR.Client.Net20.Connection;
using AspNetCore.SignalR.Client.Net20.Protocol.Handlers;
using AspNetCore.SignalR.Client.Net20.Protocol.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.SignalR.Client.Net20.Protocol
{
    internal class NewtonsoftJsonHubProtocol : IHubProtocol
    {
        private const string ResultPropertyName = "result";
        private const string ItemPropertyName = "item";
        private const string InvocationIdPropertyName = "invocationId";
        private const string StreamIdsPropertyName = "streamIds";
        private const string TypePropertyName = "type";
        private const string ErrorPropertyName = "error";
        private const string TargetPropertyName = "target";
        private const string ArgumentsPropertyName = "arguments";
        private const string HeadersPropertyName = "headers";

        private static readonly string ProtocolName = "json";
        private static readonly int ProtocolVersion = 1;
        private static readonly int ProtocolMinorVersion = 0;

        /// <summary>
        /// Gets the serializer used to serialize invocation arguments and return values.
        /// </summary>
        public JsonSerializerSettings PayloadSerializerSettings { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewtonsoftJsonHubProtocol"/> class.
        /// </summary>
        public NewtonsoftJsonHubProtocol() : this(CreateDefaultSerializerSettings())
        {
            //PayloadSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewtonsoftJsonHubProtocol"/> class.
        /// </summary>
        /// <param name="settings">The settings used to initialize the protocol.</param>
        public NewtonsoftJsonHubProtocol(JsonSerializerSettings settings)
        {
            PayloadSerializerSettings = settings;
        }

        /// <inheritdoc />
        public string Name => ProtocolName;

        /// <inheritdoc />
        public int Version => ProtocolVersion;

        /// <inheritdoc />        
        public int MinorVersion => ProtocolMinorVersion;

        /// <inheritdoc />
        public TransferFormat TransferFormat => TransferFormat.Text;

        /// <inheritdoc />
        public bool IsVersionSupported(int version)
        {
            return version == Version;
        }

        public byte[] GetMessageBytes(HubMessage message)
        {
            var json = JsonConvert.SerializeObject(message, PayloadSerializerSettings);
            Log.Debug($"HubMessage:{json}");
            return (new List<byte>(Encoding.UTF8.GetBytes(json)) { 0x1e }).ToArray();
        }

        public string GetMessageJsonString(HubMessage message)
        {
            return Encoding.UTF8.GetString(GetMessageBytes(message));
        }

        /// <summary>
        /// 尝试将json格式的消息字符串转换为 HubMessage
        /// </summary>
        /// <param name="message">json格式的消息字符串</param>
        /// <param name="hubMessage">HubMessage</param>
        /// <returns>bool</returns>
        public bool TryParseMessage(string message, out HubMessage hubMessage)
        {
            hubMessage = null;
            if (string.IsNullOrEmpty(message))
            {
                return false;
            }
            try
            {
                hubMessage = ParseMessage(message);
            }
            catch (System.Exception e)
            {
                Log.Error("尝试将json格式的消息字符串转换为 HubMessage 失败", e);
                return false;
            }
            return true;
        }

        private HubMessage ParseMessage(string message)
        {
            ReceivedMessage receivedMessage;
            try
            {
                receivedMessage = JsonConvert.DeserializeObject<ReceivedMessage>(message, PayloadSerializerSettings);
                if (receivedMessage.Type == null
                || receivedMessage.Type < HubProtocolConstants.InvocationMessageType
                || receivedMessage.Type > HubProtocolConstants.CloseMessageType)
                {
                    throw new JsonSerializationException($"type:{receivedMessage.Type}");
                }
            }
            catch (JsonSerializationException)
            {
                Log.Debug("客户端与服务器的协议不一致");
                throw;
            }
            HubMessage hubMessage = null;
            switch (receivedMessage.Type)
            {
                case HubProtocolConstants.InvocationMessageType:
                    {
                        try
                        {
                            InvocationMessage invocation = JsonConvert.DeserializeObject<InvocationMessage>(message, PayloadSerializerSettings);
                            hubMessage = invocation;
                        }
                        catch (JsonSerializationException)
                        {
                            Log.Debug($"客户端与服务器的协议不一致。消息类型：{HubProtocolConstants.InvocationMessageType.ToString()}");
                            throw;
                        }
                    }
                    break;
                case HubProtocolConstants.StreamItemMessageType:
                    {
                        try
                        {
                            StreamItemMessage streamItem = JsonConvert.DeserializeObject<StreamItemMessage>(message, PayloadSerializerSettings);
                            hubMessage = streamItem;
                        }
                        catch (JsonSerializationException)
                        {
                            Log.Debug($"客户端与服务器的协议不一致。消息类型：{HubProtocolConstants.StreamItemMessageType.ToString()}");
                            throw;
                        }
                    }
                    break;
                case HubProtocolConstants.CompletionMessageType:
                    {
                        try
                        {
                            CompletionMessage completion = JsonConvert.DeserializeObject<CompletionMessage>(message, PayloadSerializerSettings);
                            if (completion.Result != null)
                            {
                                completion.HasResult = true;
                            }
                            hubMessage = completion;
                        }
                        catch (JsonSerializationException)
                        {
                            Log.Debug($"客户端与服务器的协议不一致。消息类型：{HubProtocolConstants.CompletionMessageType.ToString()}");
                            throw;
                        }
                    }
                    break;
                case HubProtocolConstants.StreamInvocationMessageType:
                    {
                        try
                        {
                            StreamInvocationMessage streamInvocation = JsonConvert.DeserializeObject<StreamInvocationMessage>(message, PayloadSerializerSettings);
                            hubMessage = streamInvocation;
                        }
                        catch (JsonSerializationException)
                        {
                            Log.Debug($"客户端与服务器的协议不一致。消息类型：{HubProtocolConstants.StreamInvocationMessageType.ToString()}");
                            throw;
                        }
                    }
                    break;
                case HubProtocolConstants.CancelInvocationMessageType:
                    {
                        try
                        {
                            CancelInvocationMessage cancelInvocation = JsonConvert.DeserializeObject<CancelInvocationMessage>(message, PayloadSerializerSettings);
                            hubMessage = cancelInvocation;
                        }
                        catch (JsonSerializationException)
                        {
                            Log.Debug($"客户端与服务器的协议不一致。消息类型：{HubProtocolConstants.CancelInvocationMessageType.ToString()}");
                            throw;
                        }
                    }
                    break;
                case HubProtocolConstants.PingMessageType:
                    {
                        hubMessage = PingMessage.Instance;
                    }
                    break;
                case HubProtocolConstants.CloseMessageType:
                    {
                        try
                        {
                            CloseMessage close = JsonConvert.DeserializeObject<CloseMessage>(message, PayloadSerializerSettings);
                            hubMessage = close;
                        }
                        catch (JsonSerializationException)
                        {
                            Log.Debug($"客户端与服务器的协议不一致。消息类型：{HubProtocolConstants.CloseMessageType.ToString()}");
                            throw;
                        }
                    }
                    break;
                default:
                    {
                        Log.Debug("客户端与服务器的协议不一致。未知的消息类型！");
                    }
                    break;
            }
            
            return hubMessage;
        }

        internal static JsonSerializerSettings CreateDefaultSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }
    }
}
