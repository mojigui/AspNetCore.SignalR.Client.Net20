using System;

namespace AspNetCore.SignalR.Client.Net20.Protocol.Messages
{
    /// <summary>
    /// A base class for hub messages representing an invocation.
    /// </summary>
    public abstract class HubMethodInvocationMessage : HubInvocationMessage
    {
        /// <summary>
        /// Gets the target method name.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Gets the target method arguments.
        /// </summary>
        public object[] Arguments { get; set; }

        /// <summary>
        /// The target methods stream IDs.
        /// </summary>
        public string[] StreamIds { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HubMethodInvocationMessage"/> class.
        /// </summary>
        /// <param name="invocationId">The invocation ID.</param>
        /// <param name="target">The target method name.</param>
        /// <param name="arguments">The target method arguments.</param>
        /// <param name="streamIds">The target methods stream IDs.</param>
        protected HubMethodInvocationMessage(string invocationId, string target, object[] arguments, string[] streamIds)
            : this(invocationId, target, arguments)
        {
            StreamIds = streamIds;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HubMethodInvocationMessage"/> class.
        /// </summary>
        /// <param name="invocationId">The invocation ID.</param>
        /// <param name="target">The target method name.</param>
        /// <param name="arguments">The target method arguments.</param>
        protected HubMethodInvocationMessage(string invocationId, string target, object[] arguments)
            : base(invocationId)
        {
            if (string.IsNullOrEmpty(target))
            {
                throw new ArgumentException(nameof(target));
            }
            Target = target;
            Arguments = arguments;
        }

        protected HubMethodInvocationMessage()
        {
        }
    }

    /// <summary>
    /// A hub message representing a non-streaming invocation.
    /// </summary>
    public class InvocationMessage : HubMethodInvocationMessage
    {
        public InvocationMessage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvocationMessage"/> class.
        /// </summary>
        /// <param name="target">The target method name.</param>
        /// <param name="arguments">The target method arguments.</param>
        public InvocationMessage(string target, object[] arguments)
            : this(null, target, arguments)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvocationMessage"/> class.
        /// </summary>
        /// <param name="invocationId">The invocation ID.</param>
        /// <param name="target">The target method name.</param>
        /// <param name="arguments">The target method arguments.</param>
        public InvocationMessage(string invocationId, string target, object[] arguments)
            : base(invocationId, target, arguments)
        {
            Type = HubProtocolConstants.InvocationMessageType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvocationMessage"/> class.
        /// </summary>
        /// <param name="invocationId">The invocation ID.</param>
        /// <param name="target">The target method name.</param>
        /// <param name="arguments">The target method arguments.</param>
        /// <param name="streamIds">The target methods stream IDs.</param>
        public InvocationMessage(string invocationId, string target, object[] arguments, string[] streamIds)
            : base(invocationId, target, arguments, streamIds)
        {
            Type = HubProtocolConstants.InvocationMessageType;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            string args;
            string streamIds;
            try
            {
                string[] vs = new string[] { };
                if (Arguments != null)
                {
                    vs = new string[Arguments.Length];
                    for (var i = 0; i <= Arguments.Length; i++)
                    {
                        vs[i] = Arguments[i]?.ToString();
                    }
                }
                args = string.Join(", ", vs);
            }
            catch (Exception ex)
            {
                args = $"Error: {ex.Message}";
            }

            try
            {
                string[] vs = new string[] { };
                if (StreamIds != null)
                {
                    vs = new string[StreamIds.Length];
                    for (var i = 0; i <= StreamIds.Length; i++)
                    {
                        vs[i] = StreamIds[i]?.ToString();
                    }
                }
                streamIds = string.Join(", ", vs);
            }
            catch (Exception ex)
            {
                streamIds = $"Error: {ex.Message}";
            }

            return $"InvocationMessage {{ {nameof(InvocationId)}: \"{InvocationId}\", {nameof(Target)}: \"{Target}\", {nameof(Arguments)}: [ {args} ], {nameof(StreamIds)}: [ {streamIds} ] }}";
        }
    }

    /// <summary>
    /// A hub message representing a streaming invocation.
    /// </summary>
    public class StreamInvocationMessage : HubMethodInvocationMessage
    {
        public StreamInvocationMessage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamInvocationMessage"/> class.
        /// </summary>
        /// <param name="invocationId">The invocation ID.</param>
        /// <param name="target">The target method name.</param>
        /// <param name="arguments">The target method arguments.</param>
        public StreamInvocationMessage(string invocationId, string target, object[] arguments)
            : base(invocationId, target, arguments)
        {
            Type = HubProtocolConstants.StreamInvocationMessageType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamInvocationMessage"/> class.
        /// </summary>
        /// <param name="invocationId">The invocation ID.</param>
        /// <param name="target">The target method name.</param>
        /// <param name="arguments">The target method arguments.</param>
        /// <param name="streamIds">The target methods stream IDs.</param>
        public StreamInvocationMessage(string invocationId, string target, object[] arguments, string[] streamIds)
            : base(invocationId, target, arguments, streamIds)
        {
            Type = HubProtocolConstants.StreamInvocationMessageType;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            string args;
            string streamIds;
            try
            {
                string[] vs = new string[] { };
                if (Arguments != null)
                {
                    vs = new string[Arguments.Length];
                    for (var i = 0; i <= Arguments.Length; i++)
                    {
                        vs[i] = Arguments[i]?.ToString();
                    }
                }
                args = string.Join(", ", vs);
            }
            catch (Exception ex)
            {
                args = $"Error: {ex.Message}";
            }

            try
            {
                string[] vs = new string[] { };
                if (StreamIds != null)
                {
                    vs = new string[StreamIds.Length];
                    for (var i = 0; i <= StreamIds.Length; i++)
                    {
                        vs[i] = StreamIds[i]?.ToString();
                    }
                }
                streamIds = string.Join(", ", vs);
            }
            catch (Exception ex)
            {
                streamIds = $"Error: {ex.Message}";
            }

            return $"StreamInvocation {{ {nameof(InvocationId)}: \"{InvocationId}\", {nameof(Target)}: \"{Target}\", {nameof(Arguments)}: [ {args} ], {nameof(StreamIds)}: [ {streamIds} ] }}";
        }
    }
}
