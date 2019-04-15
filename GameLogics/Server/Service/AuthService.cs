using GameLogics.Server.Model;
using GameLogics.Server.Repository.Config;
using GameLogics.Server.Repository.State;
using GameLogics.Server.Repository.User;
using GameLogics.Server.Service.Token;
using GameLogics.Shared.Dao.Api;
using GameLogics.Shared.Dao.Api.Error;
using GameLogics.Shared.Dao.Auth;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Service;

namespace GameLogics.Server.Service {
	public sealed class AuthService {
		readonly ICustomLogger        _logger;
		readonly ITokenService        _token;
		readonly IUserRepository      _users;
		readonly IGameStateRepository _states;
		readonly IConfigRepository    _config;
		readonly StateInitService     _init;

		public AuthService(
			ICustomLogger logger, ITokenService token,
			IUserRepository users, IGameStateRepository states, IConfigRepository config, StateInitService init
		) {
			_logger = logger;
			_token  = token;
			_users  = users;
			_states = states;
			_config = config;
			_init   = init;
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
			var token = _token.CreateToken(user);
			var state = _states.FindOrCreate(user, s => InitAndSaveState(user, s));
			var response = new AuthResponse(token, state, _config.Get());
			_logger.Debug(this, $"User is logged in: '{user.Login}'");
			return response.AsResult();
		}

		void InitAndSaveState(UserState user, GameState state) {
			state = _init.Init(state, _config.Get());
			_states.Save(user, state.UpdateVersion());
		}
	}
}