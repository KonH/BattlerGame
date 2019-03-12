using System;

namespace UnityClient.Managers {
	[Serializable]
	public class ServerSettings {
		public ServerMode Mode;
		public string     BaseUrl;
		public int        TokenRefreshInterval;
	}
}