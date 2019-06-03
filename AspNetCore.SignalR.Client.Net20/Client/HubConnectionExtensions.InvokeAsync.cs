namespace AspNetCore.SignalR.Client.Net20.Client
{
    /// <summary>
    /// Extension methods for <see cref="HubConnectionExtensions"/>.
    /// </summary>
    public static partial class HubConnectionExtensions
    {
        /// <summary>
        /// Invokes a hub method on the server using the specified method name.
        /// </summary>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the server method to invoke.</param>
        public static void InvokeAsync(this HubConnection hubConnection, string methodName)
        {
            hubConnection.InvokeCoreAsync(methodName, new object[] { });
        }

        /// <summary>
        /// Invokes a hub method on the server using the specified method name and argument.
        /// </summary>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the server method to invoke.</param>
        /// <param name="arg1">The first argument.</param>
        public static void InvokeAsync(this HubConnection hubConnection, string methodName, object arg1)
        {
            hubConnection.InvokeCoreAsync(methodName, new[] { arg1 });
        }

        /// <summary>
        /// Invokes a hub method on the server using the specified method name and arguments.
        /// </summary>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the server method to invoke.</param>
        /// <param name="arg1">The first argument.</param>
        /// <param name="arg2">The second argument.</param>
        public static void InvokeAsync(this HubConnection hubConnection, string methodName, object arg1, object arg2)
        {
            hubConnection.InvokeCoreAsync(methodName, new[] { arg1, arg2 });
        }

        /// <summary>
        /// Invokes a hub method on the server using the specified method name and arguments.
        /// </summary>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the server method to invoke.</param>
        /// <param name="arg1">The first argument.</param>
        /// <param name="arg2">The second argument.</param>
        /// <param name="arg3">The third argument.</param>
        public static void InvokeAsync(this HubConnection hubConnection, string methodName, object arg1, object arg2, object arg3)
        {
            hubConnection.InvokeCoreAsync(methodName, new[] { arg1, arg2, arg3 });
        }

        /// <summary>
        /// Invokes a hub method on the server using the specified method name and arguments.
        /// </summary>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the server method to invoke.</param>
        /// <param name="arg1">The first argument.</param>
        /// <param name="arg2">The second argument.</param>
        /// <param name="arg3">The third argument.</param>
        /// <param name="arg4">The fourth argument.</param>
        public static void InvokeAsync(this HubConnection hubConnection, string methodName, object arg1, object arg2, object arg3, object arg4)
        {
            hubConnection.InvokeCoreAsync(methodName, new[] { arg1, arg2, arg3, arg4 });
        }

        /// <summary>
        /// Invokes a hub method on the server using the specified method name and arguments.
        /// </summary>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the server method to invoke.</param>
        /// <param name="arg1">The first argument.</param>
        /// <param name="arg2">The second argument.</param>
        /// <param name="arg3">The third argument.</param>
        /// <param name="arg4">The fourth argument.</param>
        /// <param name="arg5">The fifth argument.</param>
        public static void InvokeAsync(this HubConnection hubConnection, string methodName, object arg1, object arg2, object arg3, object arg4, object arg5)
        {
            hubConnection.InvokeCoreAsync(methodName, new[] { arg1, arg2, arg3, arg4, arg5 });
        }

        /// <summary>
        /// Invokes a hub method on the server using the specified method name and arguments.
        /// </summary>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the server method to invoke.</param>
        /// <param name="arg1">The first argument.</param>
        /// <param name="arg2">The second argument.</param>
        /// <param name="arg3">The third argument.</param>
        /// <param name="arg4">The fourth argument.</param>
        /// <param name="arg5">The fifth argument.</param>
        /// <param name="arg6">The sixth argument.</param>
        public static void InvokeAsync(this HubConnection hubConnection, string methodName, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6)
        {
            hubConnection.InvokeCoreAsync(methodName, new[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        }

        /// <summary>
        /// Invokes a hub method on the server using the specified method name and arguments.
        /// </summary>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the server method to invoke.</param>
        /// <param name="arg1">The first argument.</param>
        /// <param name="arg2">The second argument.</param>
        /// <param name="arg3">The third argument.</param>
        /// <param name="arg4">The fourth argument.</param>
        /// <param name="arg5">The fifth argument.</param>
        /// <param name="arg6">The sixth argument.</param>
        /// <param name="arg7">The seventh argument.</param>
        public static void InvokeAsync(this HubConnection hubConnection, string methodName, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7)
        {
            hubConnection.InvokeCoreAsync(methodName, new[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        }

        /// <summary>
        /// Invokes a hub method on the server using the specified method name and arguments.
        /// </summary>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the server method to invoke.</param>
        /// <param name="arg1">The first argument.</param>
        /// <param name="arg2">The second argument.</param>
        /// <param name="arg3">The third argument.</param>
        /// <param name="arg4">The fourth argument.</param>
        /// <param name="arg5">The fifth argument.</param>
        /// <param name="arg6">The sixth argument.</param>
        /// <param name="arg7">The seventh argument.</param>
        /// <param name="arg8">The eighth argument.</param>
        public static void InvokeAsync(this HubConnection hubConnection, string methodName, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8)
        {
            hubConnection.InvokeCoreAsync(methodName, new[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        }

        /// <summary>
        /// Invokes a hub method on the server using the specified method name and arguments.
        /// </summary>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the server method to invoke.</param>
        /// <param name="arg1">The first argument.</param>
        /// <param name="arg2">The second argument.</param>
        /// <param name="arg3">The third argument.</param>
        /// <param name="arg4">The fourth argument.</param>
        /// <param name="arg5">The fifth argument.</param>
        /// <param name="arg6">The sixth argument.</param>
        /// <param name="arg7">The seventh argument.</param>
        /// <param name="arg8">The eighth argument.</param>
        /// <param name="arg9">The ninth argument.</param>
        public static void InvokeAsync(this HubConnection hubConnection, string methodName, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9)
        {
            hubConnection.InvokeCoreAsync(methodName, new[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        }

        /// <summary>
        /// Invokes a hub method on the server using the specified method name and arguments.
        /// </summary>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the server method to invoke.</param>
        /// <param name="arg1">The first argument.</param>
        /// <param name="arg2">The second argument.</param>
        /// <param name="arg3">The third argument.</param>
        /// <param name="arg4">The fourth argument.</param>
        /// <param name="arg5">The fifth argument.</param>
        /// <param name="arg6">The sixth argument.</param>
        /// <param name="arg7">The seventh argument.</param>
        /// <param name="arg8">The eighth argument.</param>
        /// <param name="arg9">The ninth argument.</param>
        /// <param name="arg10">The tenth argument.</param>
        public static void InvokeAsync(this HubConnection hubConnection, string methodName, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10)
        {
            hubConnection.InvokeCoreAsync(methodName, new[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        }
    }
}
