using System;
using System.Diagnostics;
using System.Threading.Tasks;
using GameLogics.Client.Repositories;
using GameLogics.Shared.Dao.Intent;
using GameLogics.Shared.Intents;
using GameLogics.Shared.Models;
using GameLogics.Shared.Services;

namespace GameLogics.Client.Services {
	public class GameStateUpdateService {
		public event Action<GameState> OnStateUpdated = delegate {};
		
		readonly IApiService         _api;
		readonly UserRepository      _user;
		readonly GameStateRepository _state;

		public GameStateUpdateService(IApiService api, UserRepository user, GameStateRepository state) {
			_api   = api;
			_user  = user;
			_state = state;
		}

		public async Task Update(IIntent intent) {
			var state = _state.State;
			var response = await _api.Post(new IntentRequest(_user.CurrentUser.Login, state.Version, intent));
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