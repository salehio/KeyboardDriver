using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KeyboardDriver
{
    public static class Logger
    {
        public static IList<Action<string, LogSeverity>> LogHandlers = new List<Action<string, LogSeverity>>();

        public static void WriteError(object message)
        {
            WriteColor(message, ConsoleColor.Red);
            SendNotification(message, LogSeverity.Error);
        }

        public static void WriteSuccess(object message)
        {
            WriteColor(message, ConsoleColor.Green);
            SendNotification(message, LogSeverity.Success);
        }

        public static void WriteDebug(object message)
        {
            WriteColor(message, ConsoleColor.Gray);
            SendNotification(message, LogSeverity.Debug);
        }

        public static void WriteInformation(object message)
        {
            WriteColor(message, ConsoleColor.White);
            SendNotification(message, LogSeverity.Information);
        }

        public static void WriteWarning(object message)
        {
            WriteColor(message, ConsoleColor.Yellow);
            SendNotification(message, LogSeverity.Warning);
        }

        public static void WriteColor(object message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message.ToString());
            Console.ResetColor();
        }

        private static void SendNotification(object message, LogSeverity severity)
        {
            var msg = message.ToString();
            if (msg == null) return;

            foreach (var a in LogHandlers)
            {
                a(msg, severity);
            }
        }

        public enum LogSeverity
        {
            Debug,
            Success,
            Warning,
            Error,
            Information
        }
    }
}
