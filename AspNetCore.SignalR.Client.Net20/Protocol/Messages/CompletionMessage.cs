using System;

namespace AspNetCore.SignalR.Client.Net20.Protocol.Messages
{
    public class CompletionMessage : HubInvocationMessage
    {
        public string Error { get; set; }
        public object Result { get; set; }
        public bool HasResult { get; set; }

        public CompletionMessage()
        {
        }

        public CompletionMessage(string invocationId, string error, object result, bool hasResult)
            : base(invocationId)
        {
            if (error != null && result != null)
            {
                throw new ArgumentException($"Expected either '{nameof(error)}' or '{nameof(result)}' to be provided, but not both");
            }

            Error = error;
            Result = result;
            HasResult = hasResult;
        }

        public override string ToString()
        {
            var errorStr = Error == null ? "<<null>>" : $"\"{Error}\"";
            var resultField = HasResult ? $", {nameof(Result)}: {Result ?? "<<null>>"}" : string.Empty;
            return $"Completion {{ {nameof(InvocationId)}: \"{InvocationId}\", {nameof(Error)}: {errorStr}{resultField} }}";
        }

        // Static factory methods. Don't want to use constructor overloading because it will break down
        // if you need to send a payload statically-typed as a string. And because a static factory is clearer here
        public static CompletionMessage WithError(string invocationId, string error)
            => new CompletionMessage(invocationId, error, result: null, hasResult: false);

        public static CompletionMessage WithResult(string invocationId, object payload)
            => new CompletionMessage(invocationId, error: null, result: payload, hasResult: true);

        public static CompletionMessage Empty(string invocationId)
            => new CompletionMessage(invocationId, error: null, result: null, hasResult: false);
    }
}
