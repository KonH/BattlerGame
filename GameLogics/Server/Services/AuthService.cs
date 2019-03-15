using GameLogics.Server.Repositories.States;
using GameLogics.Server.Repositories.Users;
using GameLogics.Server.Services.Token;
using GameLogics.Shared.Dao.Api;
using GameLogics.Shared.Dao.Api.Errors;
using GameLogics.Shared.Dao.Auth;
using GameLogics.Shared.Services;

namespace GameLogics.Server.Services {
	public class AuthService {
		readonly ICustomLogger         _logger;
		readonly IUsersRepository      _users;
		readonly IGameStatesRepository _states;
		readonly ITokenService         _token;

		public AuthService(ICustomLogger logger, IUsersRepository users, IGameStatesRepository states, ITokenService token) {
			_logger = logger;
			_users  = users;
			_states = states;
			_token  = token;
		}

		public ApiResponse<AuthResponse> RequestToken(AuthRequest req) {
			var user = _users.Find(req.Login, req.PasswordHash);
			if ( user == null ) {
				if ( _users.Find(req.Login) == null ) {
					_logger.Debug(this, $"No user in repository with login '{req.Login}'");
				} else {
					_logger.Debug(this, $"Invalid password for user with login '{req.Login}'");
				}
				return new ClientError("Invalid login or password").AsError<AuthResponse>();
			}
			var token    = _token.CreateToken(user);
			var state    = _states.FindOrCreate(user);
			var response = new AuthResponse(token, state);
			_logger.Debug(this, $"User is logged in: '{user.Login}'");
			return response.AsResult();
		}
	}
}