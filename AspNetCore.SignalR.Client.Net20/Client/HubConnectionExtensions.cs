using System;

namespace AspNetCore.SignalR.Client.Net20.Client
{

    public static partial class HubConnectionExtensions
    {
        private static IDisposable On(this HubConnection hubConnetion, string methodName, Type[] parameterTypes, Action<object[]> handler)
        {
            return hubConnetion.On(methodName, parameterTypes, null, (parameters) =>
            {
                handler(parameters);
                return null;
            });
        }

        /// <summary>
        /// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
        /// </summary>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the hub method to define.</param>
        /// <param name="handler">The handler that will be raised when the hub method is invoked.</param>
        /// <returns>A subscription that can be disposed to unsubscribe from the hub method.</returns>
        public static IDisposable On(this HubConnection hubConnection, string methodName, Action handler)
        {
            if (hubConnection == null)
            {
                throw new ArgumentNullException(nameof(hubConnection));
            }

            return hubConnection.On(methodName, Type.EmptyTypes, args => handler());
        }

        /// <summary>
        /// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
        /// </summary>
        /// <typeparam name="T1">The first argument type.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the hub method to define.</param>
        /// <param name="handler">The handler that will be raised when the hub method is invoked.</param>
        /// <returns>A subscription that can be disposed to unsubscribe from the hub method.</returns>
        public static IDisposable On<T1>(this HubConnection hubConnection, string methodName, Action<T1> handler)
        {
            if (hubConnection == null)
            {
                throw new ArgumentNullException(nameof(hubConnection));
            }

            return hubConnection.On(methodName,
                new[] { typeof(T1) },
                args => handler((T1)args[0]));
        }

        /// <summary>
        /// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
        /// </summary>
        /// <typeparam name="T1">The first argument type.</typeparam>
        /// <typeparam name="T2">The second argument type.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the hub method to define.</param>
        /// <param name="handler">The handler that will be raised when the hub method is invoked.</param>
        /// <returns>A subscription that can be disposed to unsubscribe from the hub method.</returns>
        public static IDisposable On<T1, T2>(this HubConnection hubConnection, string methodName, Action<T1, T2> handler)
        {
            if (hubConnection == null)
            {
                throw new ArgumentNullException(nameof(hubConnection));
            }

            return hubConnection.On(methodName,
                new[] { typeof(T1), typeof(T2) },
                args => handler((T1)args[0], (T2)args[1]));
        }

        /// <summary>
        /// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
        /// </summary>
        /// <typeparam name="T1">The first argument type.</typeparam>
        /// <typeparam name="T2">The second argument type.</typeparam>
        /// <typeparam name="T3">The third argument type.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the hub method to define.</param>
        /// <param name="handler">The handler that will be raised when the hub method is invoked.</param>
        /// <returns>A subscription that can be disposed to unsubscribe from the hub method.</returns>
        public static IDisposable On<T1, T2, T3>(this HubConnection hubConnection, string methodName, Action<T1, T2, T3> handler)
        {
            if (hubConnection == null)
            {
                throw new ArgumentNullException(nameof(hubConnection));
            }

            return hubConnection.On(methodName,
                new[] { typeof(T1), typeof(T2), typeof(T3) },
                args => handler((T1)args[0], (T2)args[1], (T3)args[2]));
        }

        /// <summary>
        /// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
        /// </summary>
        /// <typeparam name="T1">The first argument type.</typeparam>
        /// <typeparam name="T2">The second argument type.</typeparam>
        /// <typeparam name="T3">The third argument type.</typeparam>
        /// <typeparam name="T4">The fourth argument type.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the hub method to define.</param>
        /// <param name="handler">The handler that will be raised when the hub method is invoked.</param>
        /// <returns>A subscription that can be disposed to unsubscribe from the hub method.</returns>
        public static IDisposable On<T1, T2, T3, T4>(this HubConnection hubConnection, string methodName, Action<T1, T2, T3, T4> handler)
        {
            if (hubConnection == null)
            {
                throw new ArgumentNullException(nameof(hubConnection));
            }

            return hubConnection.On(methodName,
                new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) },
                args => handler((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3]));
        }

        /// <summary>
        /// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
        /// </summary>
        /// <typeparam name="T1">The first argument type.</typeparam>
        /// <typeparam name="T2">The second argument type.</typeparam>
        /// <typeparam name="T3">The third argument type.</typeparam>
        /// <typeparam name="T4">The fourth argument type.</typeparam>
        /// <typeparam name="T5">The fifth argument type.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the hub method to define.</param>
        /// <param name="handler">The handler that will be raised when the hub method is invoked.</param>
        /// <returns>A subscription that can be disposed to unsubscribe from the hub method.</returns>
        public static IDisposable On<T1, T2, T3, T4, T5>(this HubConnection hubConnection, string methodName, Action<T1, T2, T3, T4, T5> handler)
        {
            if (hubConnection == null)
            {
                throw new ArgumentNullException(nameof(hubConnection));
            }

            return hubConnection.On(methodName,
                new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) },
                args => handler((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4]));
        }

        /// <summary>
        /// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
        /// </summary>
        /// <typeparam name="T1">The first argument type.</typeparam>
        /// <typeparam name="T2">The second argument type.</typeparam>
        /// <typeparam name="T3">The third argument type.</typeparam>
        /// <typeparam name="T4">The fourth argument type.</typeparam>
        /// <typeparam name="T5">The fifth argument type.</typeparam>
        /// <typeparam name="T6">The sixth argument type.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the hub method to define.</param>
        /// <param name="handler">The handler that will be raised when the hub method is invoked.</param>
        /// <returns>A subscription that can be disposed to unsubscribe from the hub method.</returns>
        public static IDisposable On<T1, T2, T3, T4, T5, T6>(this HubConnection hubConnection, string methodName, Action<T1, T2, T3, T4, T5, T6> handler)
        {
            if (hubConnection == null)
            {
                throw new ArgumentNullException(nameof(hubConnection));
            }

            return hubConnection.On(methodName,
                new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6) },
                args => handler((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5]));
        }

        /// <summary>
        /// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
        /// </summary>
        /// <typeparam name="T1">The first argument type.</typeparam>
        /// <typeparam name="T2">The second argument type.</typeparam>
        /// <typeparam name="T3">The third argument type.</typeparam>
        /// <typeparam name="T4">The fourth argument type.</typeparam>
        /// <typeparam name="T5">The fifth argument type.</typeparam>
        /// <typeparam name="T6">The sixth argument type.</typeparam>
        /// <typeparam name="T7">The seventh argument type.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the hub method to define.</param>
        /// <param name="handler">The handler that will be raised when the hub method is invoked.</param>
        /// <returns>A subscription that can be disposed to unsubscribe from the hub method.</returns>
        public static IDisposable On<T1, T2, T3, T4, T5, T6, T7>(this HubConnection hubConnection, string methodName, Action<T1, T2, T3, T4, T5, T6, T7> handler)
        {
            if (hubConnection == null)
            {
                throw new ArgumentNullException(nameof(hubConnection));
            }

            return hubConnection.On(methodName,
                new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7) },
                args => handler((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5], (T7)args[6]));
        }

        /// <summary>
        /// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
        /// </summary>
        /// <typeparam name="T1">The first argument type.</typeparam>
        /// <typeparam name="T2">The second argument type.</typeparam>
        /// <typeparam name="T3">The third argument type.</typeparam>
        /// <typeparam name="T4">The fourth argument type.</typeparam>
        /// <typeparam name="T5">The fifth argument type.</typeparam>
        /// <typeparam name="T6">The sixth argument type.</typeparam>
        /// <typeparam name="T7">The seventh argument type.</typeparam>
        /// <typeparam name="T8">The eighth argument type.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the hub method to define.</param>
        /// <param name="handler">The handler that will be raised when the hub method is invoked.</param>
        /// <returns>A subscription that can be disposed to unsubscribe from the hub method.</returns>
        public static IDisposable On<T1, T2, T3, T4, T5, T6, T7, T8>(this HubConnection hubConnection, string methodName, Action<T1, T2, T3, T4, T5, T6, T7, T8> handler)
        {
            if (hubConnection == null)
            {
                throw new ArgumentNullException(nameof(hubConnection));
            }

            return hubConnection.On(methodName,
                new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8) },
                args => handler((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5], (T7)args[6], (T8)args[7]));
        }

        /// <summary>
        /// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
        /// </summary>
        /// <typeparam name="T1">The first argument type.</typeparam>
        /// <typeparam name="T2">The second argument type.</typeparam>
        /// <typeparam name="T3">The third argument type.</typeparam>
        /// <typeparam name="T4">The fourth argument type.</typeparam>
        /// <typeparam name="T5">The fifth argument type.</typeparam>
        /// <typeparam name="T6">The sixth argument type.</typeparam>
        /// <typeparam name="T7">The seventh argument type.</typeparam>
        /// <typeparam name="T8">The eighth argument type.</typeparam>
        /// <typeparam name="T9">The nine argument type.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the hub method to define.</param>
        /// <param name="handler">The handler that will be raised when the hub method is invoked.</param>
        /// <returns>A subscription that can be disposed to unsubscribe from the hub method.</returns>
        public static IDisposable On<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this HubConnection hubConnection, string methodName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> handler)
        {
            if (hubConnection == null)
            {
                throw new ArgumentNullException(nameof(hubConnection));
            }

            return hubConnection.On(methodName,
                new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9) },
                args => handler((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5], (T7)args[6], (T8)args[7], (T9)args[8]));
        }

        /// <summary>
        /// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
        /// </summary>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the hub method to define.</param>
        /// <param name="handler">The handler that will be raised when the hub method is invoked.</param>
        /// <returns>A subscription that can be disposed to unsubscribe from the hub method.</returns>
        public static IDisposable On<TResult>(this HubConnection hubConnection, string methodName, Func<TResult> handler)
        {
            if (hubConnection == null)
            {
                throw new ArgumentNullException(nameof(hubConnection));
            }

            return hubConnection.On(methodName, Type.EmptyTypes, typeof(TResult), args => handler());
        }

        /// <summary>
        /// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
        /// </summary>
        /// <typeparam name="T1">The first argument type.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the hub method to define.</param>
        /// <param name="handler">The handler that will be raised when the hub method is invoked.</param>
        /// <returns>A subscription that can be disposed to unsubscribe from the hub method.</returns>
        public static IDisposable On<T1, TResult>(this HubConnection hubConnection, string methodName, Func<T1, TResult> handler)
        {
            if (hubConnection == null)
            {
                throw new ArgumentNullException(nameof(hubConnection));
            }

            return hubConnection.On(methodName,
                new[] { typeof(T1) }, 
                typeof(TResult),
                args => handler((T1)args[0]));
        }

        /// <summary>
        /// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
        /// </summary>
        /// <typeparam name="T1">The first argument type.</typeparam>
        /// <typeparam name="T2">The second argument type.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the hub method to define.</param>
        /// <param name="handler">The handler that will be raised when the hub method is invoked.</param>
        /// <returns>A subscription that can be disposed to unsubscribe from the hub method.</returns>
        public static IDisposable On<T1, T2, TResult>(this HubConnection hubConnection, string methodName, Func<T1, T2, TResult> handler)
        {
            if (hubConnection == null)
            {
                throw new ArgumentNullException(nameof(hubConnection));
            }

            return hubConnection.On(methodName,
                new[] { typeof(T1), typeof(T2) },
                typeof(TResult),
                args => handler((T1)args[0], (T2)args[1]));
        }

        /// <summary>
        /// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
        /// </summary>
        /// <typeparam name="T1">The first argument type.</typeparam>
        /// <typeparam name="T2">The second argument type.</typeparam>
        /// <typeparam name="T3">The third argument type.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the hub method to define.</param>
        /// <param name="handler">The handler that will be raised when the hub method is invoked.</param>
        /// <returns>A subscription that can be disposed to unsubscribe from the hub method.</returns>
        public static IDisposable On<T1, T2, T3, TResult>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, TResult> handler)
        {
            if (hubConnection == null)
            {
                throw new ArgumentNullException(nameof(hubConnection));
            }

            return hubConnection.On(methodName,
                new[] { typeof(T1), typeof(T2), typeof(T3) },
                typeof(TResult),
                args => handler((T1)args[0], (T2)args[1], (T3)args[2]));
        }

        /// <summary>
        /// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
        /// </summary>
        /// <typeparam name="T1">The first argument type.</typeparam>
        /// <typeparam name="T2">The second argument type.</typeparam>
        /// <typeparam name="T3">The third argument type.</typeparam>
        /// <typeparam name="T4">The fourth argument type.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the hub method to define.</param>
        /// <param name="handler">The handler that will be raised when the hub method is invoked.</param>
        /// <returns>A subscription that can be disposed to unsubscribe from the hub method.</returns>
        public static IDisposable On<T1, T2, T3, T4, TResult>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, T4, TResult> handler)
        {
            if (hubConnection == null)
            {
                throw new ArgumentNullException(nameof(hubConnection));
            }

            return hubConnection.On(methodName,
                new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) },
                typeof(TResult),
                args => handler((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3]));
        }

        /// <summary>
        /// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
        /// </summary>
        /// <typeparam name="T1">The first argument type.</typeparam>
        /// <typeparam name="T2">The second argument type.</typeparam>
        /// <typeparam name="T3">The third argument type.</typeparam>
        /// <typeparam name="T4">The fourth argument type.</typeparam>
        /// <typeparam name="T5">The fifth argument type.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the hub method to define.</param>
        /// <param name="handler">The handler that will be raised when the hub method is invoked.</param>
        /// <returns>A subscription that can be disposed to unsubscribe from the hub method.</returns>
        public static IDisposable On<T1, T2, T3, T4, T5, TResult>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, T4, T5, TResult> handler)
        {
            if (hubConnection == null)
            {
                throw new ArgumentNullException(nameof(hubConnection));
            }

            return hubConnection.On(methodName,
                new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) },
                typeof(TResult),
                args => handler((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4]));
        }

        /// <summary>
        /// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
        /// </summary>
        /// <typeparam name="T1">The first argument type.</typeparam>
        /// <typeparam name="T2">The second argument type.</typeparam>
        /// <typeparam name="T3">The third argument type.</typeparam>
        /// <typeparam name="T4">The fourth argument type.</typeparam>
        /// <typeparam name="T5">The fifth argument type.</typeparam>
        /// <typeparam name="T6">The sixth argument type.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the hub method to define.</param>
        /// <param name="handler">The handler that will be raised when the hub method is invoked.</param>
        /// <returns>A subscription that can be disposed to unsubscribe from the hub method.</returns>
        public static IDisposable On<T1, T2, T3, T4, T5, T6, TResult>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, T4, T5, T6, TResult> handler)
        {
            if (hubConnection == null)
            {
                throw new ArgumentNullException(nameof(hubConnection));
            }

            return hubConnection.On(methodName,
                new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6) },
                typeof(TResult),
                args => handler((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5]));
        }

        /// <summary>
        /// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
        /// </summary>
        /// <typeparam name="T1">The first argument type.</typeparam>
        /// <typeparam name="T2">The second argument type.</typeparam>
        /// <typeparam name="T3">The third argument type.</typeparam>
        /// <typeparam name="T4">The fourth argument type.</typeparam>
        /// <typeparam name="T5">The fifth argument type.</typeparam>
        /// <typeparam name="T6">The sixth argument type.</typeparam>
        /// <typeparam name="T7">The seventh argument type.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the hub method to define.</param>
        /// <param name="handler">The handler that will be raised when the hub method is invoked.</param>
        /// <returns>A subscription that can be disposed to unsubscribe from the hub method.</returns>
        public static IDisposable On<T1, T2, T3, T4, T5, T6, T7, TResult>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, TResult> handler)
        {
            if (hubConnection == null)
            {
                throw new ArgumentNullException(nameof(hubConnection));
            }

            return hubConnection.On(methodName,
                new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7) },
                typeof(TResult),
                args => handler((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5], (T7)args[6]));
        }

        /// <summary>
        /// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
        /// </summary>
        /// <typeparam name="T1">The first argument type.</typeparam>
        /// <typeparam name="T2">The second argument type.</typeparam>
        /// <typeparam name="T3">The third argument type.</typeparam>
        /// <typeparam name="T4">The fourth argument type.</typeparam>
        /// <typeparam name="T5">The fifth argument type.</typeparam>
        /// <typeparam name="T6">The sixth argument type.</typeparam>
        /// <typeparam name="T7">The seventh argument type.</typeparam>
        /// <typeparam name="T8">The eighth argument type.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the hub method to define.</param>
        /// <param name="handler">The handler that will be raised when the hub method is invoked.</param>
        /// <returns>A subscription that can be disposed to unsubscribe from the hub method.</returns>
        public static IDisposable On<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> handler)
        {
            if (hubConnection == null)
            {
                throw new ArgumentNullException(nameof(hubConnection));
            }

            return hubConnection.On(methodName,
                new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8) },
                typeof(TResult),
                args => handler((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5], (T7)args[6], (T8)args[7]));
        }

        /// <summary>
        /// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
        /// </summary>
        /// <typeparam name="T1">The first argument type.</typeparam>
        /// <typeparam name="T2">The second argument type.</typeparam>
        /// <typeparam name="T3">The third argument type.</typeparam>
        /// <typeparam name="T4">The fourth argument type.</typeparam>
        /// <typeparam name="T5">The fifth argument type.</typeparam>
        /// <typeparam name="T6">The sixth argument type.</typeparam>
        /// <typeparam name="T7">The seventh argument type.</typeparam>
        /// <typeparam name="T8">The eighth argument type.</typeparam>
        /// <typeparam name="T9">The eighth argument type.</typeparam>
        /// <param name="hubConnection">The hub connection.</param>
        /// <param name="methodName">The name of the hub method to define.</param>
        /// <param name="handler">The handler that will be raised when the hub method is invoked.</param>
        /// <returns>A subscription that can be disposed to unsubscribe from the hub method.</returns>
        public static IDisposable On<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> handler)
        {
            if (hubConnection == null)
            {
                throw new ArgumentNullException(nameof(hubConnection));
            }

            return hubConnection.On(methodName,
                new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9) },
                typeof(TResult),
                args => handler((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5], (T7)args[6], (T8)args[7], (T9)args[8]));
        }
    }
}
