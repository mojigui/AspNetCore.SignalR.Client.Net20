using System.Collections.Generic;

namespace AspNetCore.SignalR.Client.Net20.Protocol.Messages
{
    /// <summary>
    /// A base class for hub messages related to a specific invocation.
    /// </summary>
    public abstract class HubInvocationMessage : HubMessage
    {
        /// <summary>
        /// Gets or sets a name/value collection of headers.
        /// </summary>
        public IDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Gets the invocation ID.
        /// </summary>
        public string InvocationId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HubInvocationMessage"/> class.
        /// </summary>
        /// <param name="invocationId">The invocation ID.</param>
        protected HubInvocationMessage(string invocationId)
        {
            InvocationId = invocationId;
        }

        protected HubInvocationMessage()
        {
        }
    }
}
