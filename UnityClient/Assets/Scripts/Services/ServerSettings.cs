using System;

namespace UnityClient.Services {
	[Serializable]
	public sealed class ServerSettings {
		public ServerMode Mode;
		public string     BaseUrl;
		public int        TokenRefreshInterval;
		public string     EmbeddedDbName;
	}
}