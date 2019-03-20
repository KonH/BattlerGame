using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Dao.Intent;
using GameLogics.Shared.Models;
using GameLogics.Shared.Services;

namespace GameLogics.Client.Services {
	public class GameStateUpdateService {
		public event Action<GameState> OnStateUpdated   = delegate {};
		public event Action<ICommand>  OnCommandApplied = delegate(ICommand cmd) {};
		
		public GameState State => _state.State;
		
		readonly IApiService        _api;
		readonly ClientStateService _state;

		public GameStateUpdateService(IApiService api, ClientStateService state) {
			_api   = api;
			_state = state;
		}

		public bool IsValid(ICommand command) {
			return command.IsValid(_state.State, _state.Config);
		}
		
		public async Task Update(params ICommand[] commands) {
			var state = _state.State;
			var response = await _api.Post(new IntentRequest(_state.User.Login, state.Version, commands));
			if ( !response.Success ) {
				return;
			}
			var result = response.Result;
			CallCommandTree(commands);
			state.Version = result.NewVersion;
			OnStateUpdated(state);
		}

		void CallCommandTree(ICollection<ICommand> commands) {
			foreach ( var cmd in commands ) {
				var subCommands = cmd.Execute(_state.State, _state.Config);
				OnCommandApplied(cmd);
				CallCommandTree(subCommands);
			}
		}
	}
}