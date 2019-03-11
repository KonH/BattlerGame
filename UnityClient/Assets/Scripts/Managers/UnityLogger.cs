using GameLogics.Managers;

namespace UnityClient.Managers {
	public class UnityLogger : ICustomLogger {
		public void Debug(string message) {
			UnityEngine.Debug.Log(message);
		}

		public void DebugFormat(string message, params object[] args) {
			UnityEngine.Debug.LogFormat(message, args);
		}

		public void Warning(string message) {
			UnityEngine.Debug.LogWarning(message);
		}

		public void WarningFormat(string message, params object[] args) {
			UnityEngine.Debug.LogWarningFormat(message, args);
		}

		public void Error(string message) {
			UnityEngine.Debug.LogError(message);
		}

		public void ErrorFormat(string message, params object[] args) {
			UnityEngine.Debug.LogErrorFormat(message, args);
		}
	}
}