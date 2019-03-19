using System;
using System.Collections.Generic;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands {
	public class FinishLevelCommand : InternalCommand {
		public readonly bool Win;

		public FinishLevelCommand(bool win) {
			Win = win;
		}
		
		public override bool IsValid(GameState state, Config config) {
			if ( state.Level == null ) {
				return false;
			}
			return true;
		}

		public override List<ICommand> Execute(GameState state, Config config) {
			foreach ( var unit in state.Level.PlayerUnits ) {
				state.AddUnit(unit);
			}
			var reward = config.Levels[state.Level.Descriptor].Reward;
			state.Level = null;

			if ( !Win ) {
				return NoSubCommands;
			}
			
			var result = new List<ICommand>();
			foreach ( var pair in reward.Resources) {
				result.Add(new AddResourceCommand(pair.Key, pair.Value));
			}
			foreach ( var itemDesc in reward.Items ) {
				result.Add(new AddItemCommand("?", itemDesc));
			}
			foreach ( var unitDesc in reward.Units ) {
				result.Add(new AddUnitCommand("?", unitDesc, 1));
			}
			return result;
		}

		public override string ToString() {
			return $"{nameof(FinishLevelCommand)} ({Win})";
		}
	}
}