using System;

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
        /// <typeparam name="TResult">The return type of the server method.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the server method to invoke.</param>
        /// <param name="callBack">The callBack of the server method to invoke completed.</param>
        public static void InvokeAsync<TResult>(this HubConnection hubConnection, string methodName, Action<TResult, Exception> callBack)
        {
            hubConnection.InvokeCoreAsync(methodName, new object[] { }, callBack);
        }

        /// <summary>
        /// Invokes a hub method on the server using the specified method name and argument.
        /// </summary>
        /// <typeparam name="TResult">The return type of the server method.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the server method to invoke.</param>
        /// <param name="arg1">The first argument.</param>
        /// <param name="callBack">The callBack of the server method to invoke completed.</param>
        public static void InvokeAsync<TResult>(this HubConnection hubConnection, string methodName, object arg1, Action<TResult, Exception> callBack)
        {
            hubConnection.InvokeCoreAsync(methodName, new[] { arg1 }, callBack);
        }

        /// <summary>
        /// Invokes a hub method on the server using the specified method name and arguments.
        /// </summary>
        /// <typeparam name="TResult">The return type of the server method.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the server method to invoke.</param>
        /// <param name="arg1">The first argument.</param>
        /// <param name="arg2">The second argument.</param>
        /// <param name="callBack">The callBack of the server method to invoke completed.</param>
        public static void InvokeAsync<TResult>(this HubConnection hubConnection, string methodName, object arg1, object arg2, Action<TResult, Exception> callBack)
        {
            hubConnection.InvokeCoreAsync(methodName, new[] { arg1, arg2 }, callBack);
        }

        /// <summary>
        /// Invokes a hub method on the server using the specified method name and arguments.
        /// </summary>
        /// <typeparam name="TResult">The return type of the server method.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the server method to invoke.</param>
        /// <param name="arg1">The first argument.</param>
        /// <param name="arg2">The second argument.</param>
        /// <param name="arg3">The third argument.</param>
        /// <param name="callBack">The callBack of the server method to invoke completed.</param>
        public static void InvokeAsync<TResult>(this HubConnection hubConnection, string methodName, object arg1, object arg2, object arg3, Action<TResult, Exception> callBack)
        {
            hubConnection.InvokeCoreAsync(methodName, new[] { arg1, arg2, arg3 }, callBack);
        }

        /// <summary>
        /// Invokes a hub method on the server using the specified method name and arguments.
        /// </summary>
        /// <typeparam name="TResult">The return type of the server method.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the server method to invoke.</param>
        /// <param name="arg1">The first argument.</param>
        /// <param name="arg2">The second argument.</param>
        /// <param name="arg3">The third argument.</param>
        /// <param name="arg4">The fourth argument.</param>
        /// <param name="callBack">The callBack of the server method to invoke completed.</param>
        public static void InvokeAsync<TResult>(this HubConnection hubConnection, string methodName, object arg1, object arg2, object arg3, object arg4, Action<TResult, Exception> callBack)
        {
            hubConnection.InvokeCoreAsync(methodName, new[] { arg1, arg2, arg3, arg4 }, callBack);
        }

        /// <summary>
        /// Invokes a hub method on the server using the specified method name and arguments.
        /// </summary>
        /// <typeparam name="TResult">The return type of the server method.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the server method to invoke.</param>
        /// <param name="arg1">The first argument.</param>
        /// <param name="arg2">The second argument.</param>
        /// <param name="arg3">The third argument.</param>
        /// <param name="arg4">The fourth argument.</param>
        /// <param name="arg5">The fifth argument.</param>
        /// <param name="callBack">The callBack of the server method to invoke completed.</param>
        public static void InvokeAsync<TResult>(this HubConnection hubConnection, string methodName, object arg1, object arg2, object arg3, object arg4, object arg5, Action<TResult, Exception> callBack)
        {
            hubConnection.InvokeCoreAsync(methodName, new[] { arg1, arg2, arg3, arg4, arg5 }, callBack);
        }

        /// <summary>
        /// Invokes a hub method on the server using the specified method name and arguments.
        /// </summary>
        /// <typeparam name="TResult">The return type of the server method.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the server method to invoke.</param>
        /// <param name="arg1">The first argument.</param>
        /// <param name="arg2">The second argument.</param>
        /// <param name="arg3">The third argument.</param>
        /// <param name="arg4">The fourth argument.</param>
        /// <param name="arg5">The fifth argument.</param>
        /// <param name="arg6">The sixth argument.</param>
        /// <param name="callBack">The callBack of the server method to invoke completed.</param>
        public static void InvokeAsync<TResult>(this HubConnection hubConnection, string methodName, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, Action<TResult, Exception> callBack)
        {
            hubConnection.InvokeCoreAsync(methodName, new[] { arg1, arg2, arg3, arg4, arg5, arg6 }, callBack);
        }

        /// <summary>
        /// Invokes a hub method on the server using the specified method name and arguments.
        /// </summary>
        /// <typeparam name="TResult">The return type of the server method.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the server method to invoke.</param>
        /// <param name="arg1">The first argument.</param>
        /// <param name="arg2">The second argument.</param>
        /// <param name="arg3">The third argument.</param>
        /// <param name="arg4">The fourth argument.</param>
        /// <param name="arg5">The fifth argument.</param>
        /// <param name="arg6">The sixth argument.</param>
        /// <param name="arg7">The seventh argument.</param>
        /// <param name="callBack">The callBack of the server method to invoke completed.</param>
        public static void InvokeAsync<TResult>(this HubConnection hubConnection, string methodName, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, Action<TResult, Exception> callBack)
        {
            hubConnection.InvokeCoreAsync(methodName, new[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 }, callBack);
        }

        /// <summary>
        /// Invokes a hub method on the server using the specified method name and arguments.
        /// </summary>
        /// <typeparam name="TResult">The return type of the server method.</typeparam>
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
        /// <param name="callBack">The callBack of the server method to invoke completed.</param>
        public static void InvokeAsync<TResult>(this HubConnection hubConnection, string methodName, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, Action<TResult, Exception> callBack)
        {
            hubConnection.InvokeCoreAsync(methodName, new[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 }, callBack);
        }

        /// <summary>
        /// Invokes a hub method on the server using the specified method name and arguments.
        /// </summary>
        /// <typeparam name="TResult">The return type of the server method.</typeparam>
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
        /// <param name="callBack">The callBack of the server method to invoke completed.</param>
        public static void InvokeAsync<TResult>(this HubConnection hubConnection, string methodName, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, Action<TResult, Exception> callBack)
        {
            hubConnection.InvokeCoreAsync(methodName, new[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 }, callBack);
        }

        /// <summary>
        /// Invokes a hub method on the server using the specified method name and arguments.
        /// </summary>
        /// <typeparam name="TResult">The return type of the server method.</typeparam>
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
        /// <param name="callBack">The callBack of the server method to invoke completed.</param>
        public static void InvokeAsync<TResult>(this HubConnection hubConnection, string methodName, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, Action<TResult, Exception> callBack)
        {
            hubConnection.InvokeCoreAsync(methodName, new[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 }, callBack);
        }

    }
}
