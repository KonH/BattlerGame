using System.Threading.Tasks;
using GameLogics.DAO;
using GameLogics.DAO.Errors;
using GameLogics.Intents;
using GameLogics.Managers;
using GameLogics.Managers.IntentMapper;
using GameLogics.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Repositories.States;
using Server.Repositories.Users;

namespace Server.Services {
	public class IntentService {
		readonly ILogger                     _logger;
		readonly IUserRepository             _users;
		readonly IGameStateRepository        _states;
		readonly DirectIntentToCommandMapper _mapper;

		public IntentService(
			ILogger<IntentService> logger,
			IUserRepository users, IGameStateRepository states, DirectIntentToCommandMapper mapper
		) {
			_logger   = logger;
			_users    = users;
			_states   = states;
			_mapper   = mapper;
		}
		
		public async Task<CommandResponse> CreateResponse(string login, string expectedVersion, IIntent intent) {
			if ( string.IsNullOrEmpty(expectedVersion) ) {
				_logger.LogWarning($"Client should send valid state version");
				return CommandResponse.Failed(new ClientError());
			}
			var user = _users.Find(login);
			if ( user == null ) {
				_logger.LogWarning($"Can't find user with login: '{login}'");
				return CommandResponse.Failed(new InternalError());
			}
			var state = _states.Find(user);
			if ( state == null ) {
				_logger.LogWarning($"Can't find state for user with login: '{login}'");
				return CommandResponse.Failed(new InternalError());
			}
			if ( state.Version != expectedVersion ) {
				_logger.LogWarning($"Current state version don't match expected version: '{state.Version}' != '{expectedVersion}'");
				return CommandResponse.Failed(new ConflictError());
			}
			var response = await _mapper.RequestCommandsFromIntent(state, intent);
			if ( response.Success ) {
				var oldVersion = state.Version;
				foreach ( var command in response.Commands ) {
					command.Execute(state);
				}
				state.MarkAsUpdated();
				_logger.LogDebug($"State updated: version was '{oldVersion}', but now '{state.Version}'");
				response.Version = state.Version;
			}
			return response;
		}
	}
}