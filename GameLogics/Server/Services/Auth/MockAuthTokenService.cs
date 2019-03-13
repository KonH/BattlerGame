using GameLogics.Models;

namespace GameLogics.Server.Services.Auth {
	public class MockAuthTokenService : IAuthTokenService {
		public string CreateToken(User user) => "debug_token";
	}
}