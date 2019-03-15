using GameLogics.Server.Models;

namespace GameLogics.Server.Services.Token {
	public interface ITokenService {
		string CreateToken(User user);
	}
}