using GameLogics.Models;

namespace GameLogics.DAO {
	public class AuthResponse {
		public string    Token        { get; set; }
		public string    UserName     { get; set; }
		public GameState State        { get; set; }
		public string    StateVersion { get; set; } 

		public AuthResponse(string token, string userName, GameState state, string stateVersion) {
			Token        = token;
			UserName     = userName;
			State        = state;
			StateVersion = stateVersion;
		}
		
		public override string ToString() {
			return $"Token: '{Token}', UserName: '{UserName}', StateVersion: '{StateVersion}'";
		}
	}
}