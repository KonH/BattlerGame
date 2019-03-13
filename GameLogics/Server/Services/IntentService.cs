using System.Threading.Tasks;
using GameLogics.Intents;
using GameLogics.Managers;
using GameLogics.Managers.IntentMapper;
using GameLogics.Server.Repositories.States;
using GameLogics.Server.Repositories.Users;
using GameLogics.Server.Services;
using GameLogics.Server.Utils.Api;
using GameLogics.Server.Utils.Api.Errors;

namespace Server.Services {
	public class IntentService {
		readonly ICustomLogger               _logger;
		readonly ApiService                  _api;
		readonly IUsersRepository            _users;
		readonly IGameStatesRepository       _states;
		readonly DirectIntentToCommandMapper _mapper;

		public IntentService(
			ICustomLogger logger, ApiService api,
			IUsersRepository users, IGameStatesRepository states, DirectIntentToCommandMapper mapper
		) {
			_logger   = logger;
			_api      = api;
			_users    = users;
			_states   = states;
			_mapper   = mapper;
		}
		
		public async Task<ApiResponse> CreateResponse(string login, string expectedVersion, IIntent intent) {
			if ( string.IsNullOrEmpty(expectedVersion) ) {
				return _api.WithError(new ClientError("invalid state version"));
			}
			var user = _users.Find(login);
			if ( user == null ) {
				_logger.Warning(this, $"Can't find user with login: '{login}'");
				return _api.WithError(new ServerError());
			}
			var state = _states.Find(user);
			if ( state == null ) {
				_logger.Warning(this, $"Can't find state for user with login: '{login}'");
				return _api.WithError(new ServerError());
			}
			if ( state.Version != expectedVersion ) {
				_logger.Warning(this, $"Current state version don't match expected version: '{state.Version}' != '{expectedVersion}'");
				return _api.WithError(new ConflictError());
			}
			var response = await _mapper.RequestCommandsFromIntent(state, intent);
			if ( response.Success ) {
				var oldVersion = state.Version;
				foreach ( var command in response.Commands ) {
					command.Execute(state);
				}
				state.MarkAsUpdated();
				_logger.Debug(this, $"State updated: version was '{oldVersion}', but now '{state.Version}'");
				response.Version = state.Version;
			}
			return _api.Json(response);
		}
	}
}