using System;
using System.Collections.Generic;
using GameLogics.Commands;
using GameLogics.Core;
using GameLogics.Managers;

namespace GameLogics.Intents {
	public class IntentToCommandMapper {
		readonly GameState _state;
		
		public IntentToCommandMapper(IGameStateManager stateManager) {
			_state = stateManager.State;
		}

		public List<ICommand> CreateCommandsFromIntent(IIntent intent) {
			switch ( intent ) {
				case RequestResourceIntent r: return new List<ICommand> { new AddResourceCommand(r.Kind, r.Count) };
				default: throw new InvalidOperationException();
			}
		}
	}
}