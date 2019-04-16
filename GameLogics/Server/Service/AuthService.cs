using GameLogics.Server.Repository.Config;
using GameLogics.Server.Repository.State;
using GameLogics.Server.Repository.User;
using GameLogics.Server.Service.Token;
using GameLogics.Shared.Dao.Api;
using GameLogics.Shared.Dao.Api.Error;
using GameLogics.Shared.Dao.Auth;
using GameLogics.Shared.Service;
using GameLogics.Shared.Service.Time;

namespace GameLogics.Server.Service {
	public sealed class AuthService {
		readonly ICustomLogger        _logger;
		readonly ITokenService        _token;
		readonly ITimeService         _time;
		readonly IUserRepository      _users;
		readonly IGameStateRepository _states;
		readonly IConfigRepository    _config;
		readonly StateInitService     _init;

		public AuthService(
			ICustomLogger logger, ITokenService token, ITimeService time,
			IUserRepository users, IGameStateRepository states, IConfigRepository config, StateInitService init
		) {
			_logger = logger;
			_token  = token;
			_time   = time;
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
			var config = _config.Get();
			var state = _states.FindOrCreate(user, s => _init.Init(s, config));
			state.Time.LastSyncTime = _time.RealTime;
			_states.Save(user, state.UpdateVersion());
			var response = new AuthResponse(token, state, config);
			_logger.Debug(this, $"User is logged in: '{user.Login}'");
			return response.AsResult();
		}
	}
}