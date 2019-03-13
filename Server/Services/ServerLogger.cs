using GameLogics.Managers;
using Microsoft.Extensions.Logging;

namespace Server.Services {
	public class ServerLogger : ICustomLogger {
		readonly ILogger _logger;
		
		public ServerLogger(ILoggerFactory factory) {
			_logger = factory.CreateLogger("GameLogics");
		}
		
		public void Debug(object context, string message) {
			_logger.LogDebug(WithContext(context, message));
		}

		public void DebugFormat(object context, string message, params object[] args) {
			_logger.LogDebug(WithContext(context, message), args);
		}

		public void Warning(object context, string message) {
			_logger.LogWarning(WithContext(context, message));
		}

		public void WarningFormat(object context, string message, params object[] args) {
			_logger.LogWarning(WithContext(context, message), args);
		}

		public void Error(object context, string message) {
			_logger.LogError(WithContext(context, message));
		}

		public void ErrorFormat(object context, string message, params object[] args) {
			_logger.LogError(WithContext(context, message), args);
		}

		string WithContext(object context, string message) {
			return string.Format("[{0}] {1}", context.GetType().Name, message);
		}
	}
}