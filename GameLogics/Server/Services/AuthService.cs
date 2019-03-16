using GameLogics.Server.Repositories.Configs;
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
		readonly ITokenService         _token;
		readonly IUsersRepository      _users;
		readonly IGameStatesRepository _states;
		readonly IConfigRepository     _config;

		public AuthService(
			ICustomLogger logger, ITokenService token, IUsersRepository users, IGameStatesRepository states, IConfigRepository config
		) {
			_logger = logger;
			_token  = token;
			_users  = users;
			_states = states;
			_config = config;
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
			var state    = _states.FindOrCreate(user, s => _states.Save(user, s.UpdateVersion()));
			var response = new AuthResponse(token, state, _config.Get());
			_logger.Debug(this, $"User is logged in: '{user.Login}'");
			return response.AsResult();
		}
	}
}