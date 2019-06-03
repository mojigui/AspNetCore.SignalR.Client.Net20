using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.SignalR.Client.Net20.Protocol
{
    public static class HubProtocolConstants
    {
        /// <summary>
        /// Represents the invocation message type.
        /// </summary>
        public const int InvocationMessageType = 1;

        /// <summary>
        /// Represents the stream item message type.
        /// </summary>
        public const int StreamItemMessageType = 2;

        /// <summary>
        /// Represents the completion message type.
        /// </summary>
        public const int CompletionMessageType = 3;

        /// <summary>
        /// Represents the stream invocation message type.
        /// </summary>
        public const int StreamInvocationMessageType = 4;

        /// <summary>
        /// Represents the cancel invocation message type.
        /// </summary>
        public const int CancelInvocationMessageType = 5;

        /// <summary>
        /// Represents the ping message type.
        /// </summary>
        public const int PingMessageType = 6;

        /// <summary>
        /// Represents the close message type.
        /// </summary>
        public const int CloseMessageType = 7;
    }
}
