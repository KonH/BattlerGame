using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameLogics.Client.Service.Event;
using GameLogics.Shared.Command.Base;
using GameLogics.Shared.Dao.Intent;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Service;
using GameLogics.Shared.Utils;

namespace GameLogics.Client.Service {
	public sealed class GameStateUpdateService {
		public event Action<GameState> OnStateUpdated = delegate {};
		
		public GameState State => _state.State;

		readonly ICustomLogger      _logger;
		readonly IApiService        _api;
		readonly ClientStateService _state;
		
		Dictionary<Type, EventTaskBaseHandler> _handlers = new Dictionary<Type, EventTaskBaseHandler>();

		public GameStateUpdateService(ICustomLogger logger, IApiService api, ClientStateService state) {
			_logger = logger;
			_api    = api;
			_state  = state;
		}

		public void AddHandler<T>(Func<T, Task> handler) where T : ICommand {			
			var type = typeof(T);
			if ( !_handlers.TryGetValue(type, out var container) ) {
				container = new EventTaskHandler<T>();
				_handlers.Add(type, container);
			}
			container.Add(handler);
		}

		public void RemoveHandler<T>(Func<T, Task> handler) where T : ICommand {
			var type      = typeof(T);
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
				var container = _handlers.GetOrDefault(item.Command.GetType());
				if ( container != null ) {
					var handlers = container.GetHandlers();
					foreach ( var handler in handlers ) {
						await container.Invoke(handler, item.Command);
						OnStateUpdated(state);
					}
				}
				OnStateUpdated(state);
			}
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