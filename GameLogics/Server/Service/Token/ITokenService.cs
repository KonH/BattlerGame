using GameLogics.Server.Model;

namespace GameLogics.Server.Service.Token {
	public interface ITokenService {
		string CreateToken(UserState user);
	}
}