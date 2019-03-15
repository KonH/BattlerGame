using GameLogics.Server.Models;

namespace GameLogics.Server.Services.Token {
	public class MockTokenService : ITokenService {
		public string CreateToken(User user) => "debug_token";
	}
}