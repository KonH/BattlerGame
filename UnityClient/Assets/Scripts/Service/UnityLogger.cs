using GameLogics.Shared.Service;

namespace UnityClient.Service {
	public sealed class UnityLogger : ICustomLogger {
		public void Debug(object context, string message) {
			UnityEngine.Debug.Log(WithContext(context, message));
		}

		public void DebugFormat(object context, string message, params object[] args) {
			UnityEngine.Debug.LogFormat(WithContext(context, message), args);
		}

		public void Warning(object context, string message) {
			UnityEngine.Debug.LogWarning(WithContext(context, message));
		}

		public void WarningFormat(object context, string message, params object[] args) {
			UnityEngine.Debug.LogWarningFormat(WithContext(context, message), args);
		}

		public void Error(object context, string message) {
			UnityEngine.Debug.LogError(WithContext(context, message));
		}

		public void ErrorFormat(object context, string message, params object[] args) {
			UnityEngine.Debug.LogErrorFormat(WithContext(context, message), args);
		}

		string WithContext(object context, string message) {
			return string.Format("[{0}] {1}", context, message);
		}
	}
}