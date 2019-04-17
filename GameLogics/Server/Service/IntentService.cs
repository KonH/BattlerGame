using GameLogics.Server.Repository.Config;
using GameLogics.Server.Repository.State;
using GameLogics.Server.Repository.User;
using GameLogics.Shared.Command.Base;
using GameLogics.Shared.Dao.Api;
using GameLogics.Shared.Dao.Api.Error;
using GameLogics.Shared.Dao.Intent;
using GameLogics.Shared.Service;
using GameLogics.Shared.Service.Time;

namespace GameLogics.Server.Service {
	public sealed class IntentService {
		readonly EnvironmentService   _env;
		readonly ICustomLogger        _logger;
		readonly ITimeService         _time;
		readonly IUserRepository      _users;
		readonly IGameStateRepository _states;
		readonly IConfigRepository    _config;

		public IntentService(EnvironmentService env, ICustomLogger logger, ITimeService time, IUserRepository users, IGameStateRepository states, IConfigRepository config) {
			_env    = env;
			_logger = logger;
			_time   = time;
			_users  = users;
			_states = states;
			_config = config;
		}
		
		public ApiResponse<IntentResponse> CreateCommands(IntentRequest req) {
			if ( string.IsNullOrEmpty(req.ExpectedVersion) ) {
				return new ClientError("invalid state version").AsError<IntentResponse>();
			}
			var user = _users.Find(req.Login);
			if ( user == null ) {
				_logger.Warning(this, $"Can't find user with login: '{req.Login}'");
				return new ServerError().AsError<IntentResponse>();
			}
			var state = _states.Find(user);
			if ( state == null ) {
				_logger.Warning(this, $"Can't find state for user with login: '{req.Login}'");
				return new ServerError().AsError<IntentResponse>();
			}
			if ( state.Version != req.ExpectedVersion ) {
				_logger.Warning(this, $"Current state version don't match expected version: '{state.Version}' != '{req.ExpectedVersion}'");
				return new ConflictError($"Current state version is '{state.Version}'").AsError<IntentResponse>();
			}
			var config = _config.Get();
			var command = req.Command;
			if ( !IsValidAsFirstCommand(command) ) {
				return new ClientError($"Trying to execute internal command: '{command}'").AsError<IntentResponse>();
			}
			var runner = new CommandRunner(_time.RealTime - state.Time.LastSyncTime, command, state, config);
			foreach ( var item in runner ) {
				if ( (item.Command is IDebugCommand) && !_env.IsDebugMode ) {
					return new ClientError($"Invalid debug command: '{item.Command}'").AsError<IntentResponse>();
				}
				if ( !item.IsValid() ) {
					return new ClientError($"Invalid command: '{item.Command}'").AsError<IntentResponse>();
				}
				item.Execute();
			}
			var oldVersion = state.Version;
			state.UpdateVersion();
			state = _states.Save(user, state);
			_logger.Debug(this, $"State updated: version was '{oldVersion}', changed to '{state.Version}'");
			return new IntentResponse(state.Version).AsResult();
		}

		public static bool IsValidAsFirstCommand(ICommand command) {
			return !(command is IInternalCommand);
		}
	}
}