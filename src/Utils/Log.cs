namespace Mundos {
    public static class Log {
        internal static bool _debug = false;

        /// <summary>
        /// Writes a debug message to the console.
        /// This will only write the message if the debug flag is set to true.
        /// </summary>
        /// <param name="message">The debug message to write.</param>
        public static void Debug(string message) {
            if (!_debug) return;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[DEBUG] {message}");
            Console.ResetColor();
        }

        /// <summary>
        /// Writes an informational message to the console.
        /// This will write the message in white.
        /// </summary>
        /// <param name="message">The message to be written.</param>
        public static void Info(string message) {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[INFO] {message}");
            Console.ResetColor();
        }

        /// <summary>
        /// Writes a warning message to the console.
        /// This will write the message in yellow.
        /// </summary>
        /// <param name="message">The warning message to write.</param>
        public static void Warning(string message) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[WARNING] {message}");
            Console.ResetColor();
        }

        /// <summary>
        /// Writes an error message to the console.
        /// This will write the message in red.
        /// </summary>
        /// <param name="message">The error message to write.</param>
        public static void Error(string message) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] {message}");
            Console.ResetColor();
        }

        /// <summary>
        /// Writes a critical message to the console.
        /// This will write the message in dark red.
        /// </summary>
        /// <param name="message">The critical message to write.</param>
        public static void Critical(string message) {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"[CRITICAL] {message}");
            Console.ResetColor();
        }

        /// <summary>
        /// Prints a success message to the console with green color.
        /// This will write the message in green.
        /// </summary>
        /// <param name="message">The success message to be printed.</param>
        public static void Success(string message) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[SUCCESS] {message}");
            Console.ResetColor();
        }

        /// <summary>
        /// Writes a line of text to the console.
        /// This will write the message as a regular line of text.
        /// </summary>
        /// <param name="message">The text to write.</param>
        public static void WriteLine(string message) {
            Console.ResetColor();
            Console.WriteLine(message);
        }

    }
}