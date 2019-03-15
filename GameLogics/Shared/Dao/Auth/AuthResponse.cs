using GameLogics.Shared.Models;

namespace GameLogics.Shared.Dao.Auth {
	public class AuthResponse {
		public string    Token { get; set; }
		public GameState State { get; set; }

		public AuthResponse(string token, GameState state) {
			Token = token;
			State = state;
		}
		
		public override string ToString() {
			return $"Token: '{Token}', State: '{State.Version}''";
		}
	}
}