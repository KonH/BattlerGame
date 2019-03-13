using GameLogics.Models;

namespace GameLogics.Server.Services.Auth {
	public interface IAuthTokenService {
		string CreateToken(User user);
	}
}