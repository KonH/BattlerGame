using System;

namespace UnityClient.Service {
	[Serializable]
	public sealed class ServerSettings {
		public ServerMode Mode;
		public string     BaseUrl;
		public int        TokenRefreshInterval;
		public string     EmbeddedDbName;
		public bool       IsDebugMode;
	}
}