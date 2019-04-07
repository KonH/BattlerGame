using System;
using GameLogics.Shared.Services;

namespace ConsoleClient {
	public sealed class ConsoleLogger : ICustomLogger {
		public void Debug(object context, string message) {
			WriteLine(ConsoleColor.Green, context, message);
		}

		public void DebugFormat(object context, string message, params object[] args) {
			WriteLine(ConsoleColor.Green, context, message, args);
		}

		public void Warning(object context, string message) {
			WriteLine(ConsoleColor.Yellow, context, message);
		}

		public void WarningFormat(object context, string message, params object[] args) {
			WriteLine(ConsoleColor.Yellow, context, message, args);
		}

		public void Error(object context, string message) {
			WriteLine(ConsoleColor.Red, context, message);
		}

		public void ErrorFormat(object context, string message, params object[] args) {
			WriteLine(ConsoleColor.Red, context, message, args);
		}

		void WriteLine(ConsoleColor color, object context, string message) {
			WriteWithColor(color, context, message);
		}

		void WriteLine(ConsoleColor color, object context, string message, params object[] args) {
			WriteWithColor(color, context, string.Format(message, args));
		}

		void WriteWithColor(ConsoleColor color, object context, string message) {
			WriteWithColor(color, $"[{context.GetType().Name}] {message}");
		}

		public static void WriteWithColor(ConsoleColor color, string message) {
			var prevColor = Console.ForegroundColor;
			Console.ForegroundColor = color;
			Console.WriteLine(message);
			Console.ForegroundColor = prevColor;
		}
	}
}