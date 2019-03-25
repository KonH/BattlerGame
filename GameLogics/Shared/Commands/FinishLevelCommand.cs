using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands {
	public class FinishLevelCommand : IInternalCommand {
		public readonly bool Win;

		public FinishLevelCommand(bool win) {
			Win = win;
		}
		
		public bool IsValid(GameState state, Config config) {
			if ( state.Level == null ) {
				return false;
			}
			return true;
		}

		public void Execute(GameState state, Config config, ICommandBuffer buffer) {
			foreach ( var unit in state.Level.PlayerUnits ) {
				state.AddUnit(unit);
			}
			var reward = config.Levels[state.Level.Descriptor].Reward;
			state.Level = null;

			if ( !Win ) {
				return;
			}
			
			foreach ( var pair in reward.Resources ) {
				buffer.Add(new AddResourceCommand(pair.Key, pair.Value));
			}
			foreach ( var itemDesc in reward.Items ) {
				buffer.Add(new AddItemCommand(state.NewEntityId(), itemDesc));
			}
			foreach ( var unitDesc in reward.Units ) {
				buffer.Add(new AddUnitCommand(state.NewEntityId(), unitDesc));
			}
		}

		public override string ToString() {
			return $"{nameof(FinishLevelCommand)} ({Win})";
		}
	}
}