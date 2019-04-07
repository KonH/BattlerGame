using GameLogics.Server.Models;

namespace GameLogics.Server.Services.Token {
	public sealed class MockTokenService : ITokenService {
		public string CreateToken(User user) => "debug_token";
	}
}