using System;
using GameLogics.Managers;

namespace ConsoleClient {
	public class ConsoleLogger : ICustomLogger {
		public void Debug(string message) {
			WriteLine(ConsoleColor.Green, message);
		}

		public void DebugFormat(string message, params object[] args) {
			WriteLine(ConsoleColor.Green, message, args);
		}

		public void Warning(string message) {
			WriteLine(ConsoleColor.Yellow, message);
		}

		public void WarningFormat(string message, params object[] args) {
			WriteLine(ConsoleColor.Yellow, message, args);
		}

		public void Error(string message) {
			WriteLine(ConsoleColor.Red, message);
		}

		public void ErrorFormat(string message, params object[] args) {
			WriteLine(ConsoleColor.Red, message, args);
		}

		void WriteLine(ConsoleColor color, string message) {
			WriteWithColor(color, message);
		}

		void WriteLine(ConsoleColor color, string message, params object[] args) {
			WriteWithColor(color, string.Format(message, args));
		}

		void WriteWithColor(ConsoleColor color, string message) {
			var prevColor = Console.ForegroundColor;
			Console.ForegroundColor = color;
			Console.WriteLine(message);
			Console.ForegroundColor = prevColor;
		}
	}
}