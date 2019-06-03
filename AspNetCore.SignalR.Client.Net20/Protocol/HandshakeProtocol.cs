using AspNetCore.SignalR.Client.Net20.Protocol.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AspNetCore.SignalR.Client.Net20.Protocol
{
    internal class HandshakeProtocol
    {
        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static bool TryParseHandshakeResponseMessage(string message, out HandshakeResponseMessage responseMessage)
        {
            if ("{}".Equals(message))
            {
                responseMessage = HandshakeResponseMessage.Empty;
                return true;
            }
            try
            {
                responseMessage = JsonConvert.DeserializeObject<HandshakeResponseMessage>(message, _serializerSettings);
            }
            catch (JsonSerializationException)
            {
                Log.Debug("客户端与服务器的协议不一致");
                responseMessage = null;
                return false;
            }

            if (responseMessage.Type != null
                && responseMessage.Type >= HubProtocolConstants.InvocationMessageType
                && responseMessage.Type <= HubProtocolConstants.CloseMessageType)
            {
                // a handshake response does not have a type
                Log.Debug($"a handshake response does not have a type. type={responseMessage.Type}");
                responseMessage = null;
                return false;
            }
            return true;
        }
    }
}
