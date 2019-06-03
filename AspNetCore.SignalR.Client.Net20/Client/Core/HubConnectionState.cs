namespace AspNetCore.SignalR.Client.Net20.Client
{
    /// <summary>
    /// Describes the current state of the <see cref="HubConnection"/> to the server.
    /// </summary>
    public enum HubConnectionState
    {
        /// <summary>
        /// The hub connection is disconnected.
        /// </summary>
        Disconnected,
        /// <summary>
        /// The hub connection is connected.
        /// </summary>
        Connected
    }
}
