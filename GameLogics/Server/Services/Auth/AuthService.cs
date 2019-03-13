using GameLogics.DAO;
using GameLogics.Managers;
using GameLogics.Models;
using GameLogics.Server.Repositories.States;
using GameLogics.Server.Repositories.Users;
using GameLogics.Server.Utils.Api;
using GameLogics.Server.Utils.Api.Errors;

namespace GameLogics.Server.Services.Auth {
	public class AuthService {
		readonly ICustomLogger         _logger;
		readonly ApiService            _api;
		readonly IUsersRepository      _users;
		readonly IGameStatesRepository _states;
		readonly IAuthTokenService     _tokenService;

		public AuthService(ICustomLogger logger, ApiService api, IUsersRepository users, IGameStatesRepository states, IAuthTokenService tokenService) {
			_logger       = logger;
			_api          = api;
			_users        = users;
			_states       = states;
			_tokenService = tokenService;
		}

		public ApiResponse RequestToken(User user) {
			var storedUser = _users.Find(user.Login, user.PasswordHash);
			if ( storedUser == null ) {
				if ( _users.Find(user.Login) == null ) {
					_logger.Debug(this, $"No user in repository with login '{user.Login}'");
				} else {
					_logger.Debug(this, $"Invalid password for user with login '{user.Login}'");
				}
				return _api.WithError(new ClientError("Invalid login or password"));
			}
			var token    = _tokenService.CreateToken(storedUser);
			var state    = _states.FindOrCreate(storedUser);
			var response = new AuthResponse(token, storedUser.Login, state, state.Version);
			_logger.Debug(this, $"User is logged in: '{storedUser.Login}'");
			return _api.Json(response);
		}
	}
}