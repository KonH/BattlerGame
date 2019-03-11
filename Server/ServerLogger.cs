using GameLogics.Managers;
using Microsoft.Extensions.Logging;

namespace Server {
	public class ServerLogger : ICustomLogger {
		readonly ILogger _logger;
		
		public ServerLogger(ILoggerFactory factory) {
			_logger = factory.CreateLogger("GameLogics");
		}
		
		public void Debug(string message) {
			_logger.LogDebug(message);
		}

		public void DebugFormat(string message, params object[] args) {
			_logger.LogDebug(message, args);
		}

		public void Warning(string message) {
			_logger.LogWarning(message);
		}

		public void WarningFormat(string message, params object[] args) {
			_logger.LogWarning(message, args);
		}

		public void Error(string message) {
			_logger.LogError(message);
		}

		public void ErrorFormat(string message, params object[] args) {
			_logger.LogError(message, args);
		}
	}
}