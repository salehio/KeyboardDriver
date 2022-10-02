using System;

namespace KeyboardDriver
{
    internal static class Logger
    {
        #region console helpers
        public static void WriteError(object message)
        {
            WriteColor(message, ConsoleColor.Red);
        }

        public static void WriteSuccess(object message)
        {
            WriteColor(message, ConsoleColor.Green);
        }

        public static void WriteDebug(object message)
        {
            WriteColor(message, ConsoleColor.Gray);
        }

        public static void WriteWarning(object message)
        {
            WriteColor(message, ConsoleColor.Yellow);
        }

        public static void WriteColor(object message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message.ToString());
            Console.ResetColor();
        }
        #endregion
    }
}
