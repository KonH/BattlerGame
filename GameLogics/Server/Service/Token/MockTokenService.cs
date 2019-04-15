using GameLogics.Server.Model;

namespace GameLogics.Server.Service.Token {
	public sealed class MockTokenService : ITokenService {
		public string CreateToken(UserState user) => "debug_token";
	}
}