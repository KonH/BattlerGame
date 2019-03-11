using JetBrains.Annotations;

namespace GameLogics.Managers {
	public interface ICustomLogger {
		void Debug(string message);
		
		[StringFormatMethod("message")]
		void DebugFormat(string message, params object[] args);
		
		void Warning(string message);
		
		[StringFormatMethod("message")]
		void WarningFormat(string message, params object[] args);
		
		void Error(string message);
		
		[StringFormatMethod("message")]
		void ErrorFormat(string message, params object[] args);
	}
}