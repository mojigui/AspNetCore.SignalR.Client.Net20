using System;
using System.ComponentModel;

namespace AspNetCore.SignalR.Client.Net20
{
    internal class Log
    {
        public static void Info(string message)
        {
            ConsoleWriteLine("Info", message);
        }

        public static void Debug(string message)
        {
#if DEBUG
            ConsoleWriteLine("Debug", message);
#endif
        }

        public static void Error(string message, Exception e)
        {
            ConsoleWriteLine("Error", message);
            if (e != null)
            {
                ConsoleWriteLine("Error", e.Message);
            }
        }

        private static void ConsoleWriteLine(string env, string message)
        {
            Console.WriteLine($"AspNetCore.SignalR.Client.Net20[{env}]: {message}");
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
