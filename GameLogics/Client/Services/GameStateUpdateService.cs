using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Dao.Intent;
using GameLogics.Shared.Models;
using GameLogics.Shared.Services;
using GameLogics.Shared.Utils;

namespace GameLogics.Client.Services {
	public class GameStateUpdateService {
		public event Action<GameState> OnStateUpdated = delegate {};
		
		public GameState State => _state.State;

		readonly ICustomLogger      _logger;
		readonly IApiService        _api;
		readonly ClientStateService _state;
		
		Dictionary<Type, List<Func<ICommand, Task>>> _handlers = new Dictionary<Type, List<Func<ICommand, Task>>>();

		public GameStateUpdateService(ICustomLogger logger, IApiService api, ClientStateService state) {
			_logger = logger;
			_api    = api;
			_state  = state;
		}

		public void AddHandler<T>(Func<ICommand, Task> handler) where T : ICommand {
			var type = typeof(T);
			var container = _handlers.GetOrCreate(type);
			container.Add(handler);
		}

		public void RemoveHandler<T>(Func<ICommand, Task> handler) where T : ICommand {
			var type = typeof(T);
			var container = _handlers.GetOrDefault(type);
			container?.Remove(handler);
		}

		public bool IsValid(ICommand command) {
			return command.IsValid(_state.State, _state.Config);
		}
		
		public async Task Update(ICommand command) {
			var state  = _state.State;
			var config = _state.Config;
			var runner = new CommandRunner(command, state, config);
			foreach ( var item in runner ) {
				_logger.DebugFormat(this, "Start executing command: {0}", item.Command);
				if ( !item.IsValid() ) {
					_logger.ErrorFormat(this, "Command is invalid: {0}", item.Command);
					return;
				}
				item.Execute();
				_logger.DebugFormat(this, "End executing command: {0}", item.Command);
				var handlers = _handlers.GetOrDefault(item.Command.GetType());
				if ( handlers != null ) {
					foreach ( var handler in handlers ) {
						await handler.Invoke(item.Command);
					}
				}
			}
			OnStateUpdated(state);
			var response = await _api.Post(new IntentRequest(_state.User.Login, state.Version, command));
			if ( !response.Success ) {
				_logger.Error(this, "State declined from server.");
				return;
			}
			var result = response.Result;
			state.Version = result.NewVersion;
			_logger.Debug(this, "State approved from server.");
		}
	}
}