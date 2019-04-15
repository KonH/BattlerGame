using JetBrains.Annotations;

namespace GameLogics.Shared.Service {
	public interface ICustomLogger {
		void Debug(object context, string message);
		
		[StringFormatMethod("message")]
		void DebugFormat(object context, string message, params object[] args);
		
		void Warning(object context, string message);
		
		[StringFormatMethod("message")]
		void WarningFormat(object context, string message, params object[] args);
		
		void Error(object context, string message);
		
		[StringFormatMethod("message")]
		void ErrorFormat(object context, string message, params object[] args);
	}
}