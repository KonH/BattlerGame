using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Dao.Auth {
	public class AuthResponse {
		public string    Token  { get; set; }
		public GameState State  { get; set; }
		public Config    Config { get; set; }

		public AuthResponse(string token, GameState state, Config config) {
			Token  = token;
			State  = state;
			Config = config;
		}
		
		public override string ToString() {
			return $"Token: '{Token}', State: '{State.Version}', Config: '{Config.Version}'";
		}
	}
}