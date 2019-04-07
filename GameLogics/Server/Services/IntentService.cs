using GameLogics.Server.Repositories.Configs;
using GameLogics.Server.Repositories.States;
using GameLogics.Server.Repositories.Users;
using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Dao.Api;
using GameLogics.Shared.Dao.Api.Errors;
using GameLogics.Shared.Dao.Intent;
using GameLogics.Shared.Services;

namespace GameLogics.Server.Services {
	public sealed class IntentService {
		readonly ICustomLogger         _logger;
		readonly IUsersRepository      _users;
		readonly IGameStatesRepository _states;
		readonly IConfigRepository     _config;

		public IntentService(ICustomLogger logger, IUsersRepository users, IGameStatesRepository states, IConfigRepository config) {
			_logger = logger;
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
			var runner = new CommandRunner(command, state, config);
			foreach ( var item in runner ) {
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