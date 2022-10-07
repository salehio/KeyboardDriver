using System;
using System.Windows.Forms;

namespace KeyboardDriver
{
    internal static class Logger
    {
        public static NotifyIcon? TrayIcon { get; set; } = default;

        public static void WriteError(object message)
        {
            WriteColor(message, ConsoleColor.Red);
            SendNotification(message, ToolTipIcon.Error);
        }

        public static void WriteSuccess(object message)
        {
            WriteColor(message, ConsoleColor.Green);
            SendNotification(message, ToolTipIcon.Info);
        }

        public static void WriteDebug(object message)
        {
            WriteColor(message, ConsoleColor.Gray);
            SendNotification(message, ToolTipIcon.None);
        }

        public static void WriteWarning(object message)
        {
            WriteColor(message, ConsoleColor.Yellow);
            SendNotification(message, ToolTipIcon.Warning);
        }

        public static void WriteColor(object message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message.ToString());
            Console.ResetColor();
        }

        private static void SendNotification(object message, ToolTipIcon icon)
        {
            TrayIcon?.ShowBalloonTip(2000, "Logger", message.ToString(), icon);
        }
    }
}
