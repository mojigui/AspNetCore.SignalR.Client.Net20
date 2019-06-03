namespace AspNetCore.SignalR.Client.Net20.Protocol.Messages
{
    /// <summary>
    /// The message sent when closing a connection.
    /// </summary>
    public class CloseMessage : HubMessage
    {
        /// <summary>
        /// An empty close message with no error.
        /// </summary>
        public static readonly CloseMessage Empty = new CloseMessage(null);

        /// <summary>
        /// Gets the optional error message.
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloseMessage"/> class with an optional error message.
        /// </summary>
        /// <param name="error">An optional error message.</param>
        public CloseMessage(string error)
        {
            Type = HubProtocolConstants.CloseMessageType;
            Error = error;
        }

        public CloseMessage()
        {
        }
    }
}
