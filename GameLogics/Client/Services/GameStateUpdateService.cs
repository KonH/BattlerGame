using System;
using System.Threading.Tasks;
using GameLogics.Shared.Dao.Intent;
using GameLogics.Shared.Intents;
using GameLogics.Shared.Models;
using GameLogics.Shared.Services;

namespace GameLogics.Client.Services {
	public class GameStateUpdateService {
		public event Action<GameState> OnStateUpdated = delegate {};
		
		public GameState State => _state.State;
		
		readonly IApiService        _api;
		readonly ClientStateService _state;

		public GameStateUpdateService(IApiService api, ClientStateService state) {
			_api   = api;
			_state = state;
		}

		public async Task Update(IIntent intent) {
			var state = _state.State;
			var response = await _api.Post(new IntentRequest(_state.User.Login, state.Version, intent));
			if ( !response.Success ) {
				return;
			}
			var result = response.Result;
			foreach ( var cmd in result.Commands ) {	
				cmd.Execute(state);
			}
			state.Version = result.NewVersion;
			OnStateUpdated(state);
		}
	}
}