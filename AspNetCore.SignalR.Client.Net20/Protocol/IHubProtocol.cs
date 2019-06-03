using AspNetCore.SignalR.Client.Net20.Client;
using AspNetCore.SignalR.Client.Net20.Connection;
using AspNetCore.SignalR.Client.Net20.Protocol.Handlers;
using AspNetCore.SignalR.Client.Net20.Protocol.Messages;
using System.Collections.Generic;

namespace AspNetCore.SignalR.Client.Net20.Protocol
{
    /// <summary>
    /// A protocol abstraction for communicating with SignalR hubs.
    /// </summary>
    public interface IHubProtocol
    {
        /// <summary>
        /// Gets the name of the protocol. The name is used by SignalR to resolve the protocol between the client and server.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the major version of the protocol.
        /// </summary>
        int Version { get; }

        /// <summary>
        /// Gets the minor version of the protocol.
        /// </summary>
        int MinorVersion { get; }

        /// <summary>
        /// Gets the transfer format of the protocol.
        /// </summary>
        TransferFormat TransferFormat { get; }

        /// <summary>
        /// Gets a value indicating whether the protocol supports the specified version.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns>A value indicating whether the protocol supports the specified version.</returns>
        bool IsVersionSupported(int version);

        /// <summary>
        /// Converts the specified <see cref="HubMessage"/> to its serialized representation.
        /// </summary>
        /// <param name="message">The message to convert.</param>
        /// <returns>The serialized bytes of the message.</returns>
        byte[] GetMessageBytes(HubMessage message);

        /// <summary>
        /// Try Parse HubMessage
        /// </summary>
        /// <param name="message">The message to parse.</param>
        /// <param name="hubMessage">HubMessage</param>
        /// <returns>bool</returns>
        bool TryParseMessage(string message, out HubMessage hubMessage);
    }
}
