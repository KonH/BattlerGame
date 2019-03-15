using GameLogics.Server.Repositories.States;
using GameLogics.Server.Repositories.Users;
using GameLogics.Shared.Dao.Api;
using GameLogics.Shared.Dao.Api.Errors;
using GameLogics.Shared.Dao.Intent;
using GameLogics.Shared.Services;

namespace GameLogics.Server.Services {
	public class IntentService {
		readonly ICustomLogger         _logger;
		readonly IUsersRepository      _users;
		readonly IGameStatesRepository _states;
		readonly IntentToCommandMapper _mapper;

		public IntentService(ICustomLogger logger, IUsersRepository users, IGameStatesRepository states, IntentToCommandMapper mapper) {
			_logger = logger;
			_users  = users;
			_states = states;
			_mapper = mapper;
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
			var commands = _mapper.CreateCommandsFromIntent(state, req.Intent);
			foreach ( var command in commands ) {
				command.Execute(state);
			}
			var oldVersion = state.Version;
			state = _states.Save(user, state);
			_logger.Debug(this, $"State updated: version was '{oldVersion}', changed to '{state.Version}'");
			return new IntentResponse(state.Version, commands).AsResult();
		}
	}
}