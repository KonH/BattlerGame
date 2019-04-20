using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;

namespace GameLogics.Shared.Dao.Auth {
	public sealed class AuthResponse {
		public string     Token  { get; set; }
		public GameState  State  { get; set; }
		public ConfigRoot Config { get; set; }

		public AuthResponse(string token, GameState state, ConfigRoot config) {
			Token  = token;
			State  = state;
			Config = config;
		}
		
		public override string ToString() {
			return $"Token: '{Token}', State: '{State.Version}', Config: '{Config.Version}'";
		}
	}
}