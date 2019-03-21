using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands {
	public class FinishLevelCommand : InternalCommand {
		public readonly bool Win;

		public FinishLevelCommand(bool win) {
			Win = win;
		}
		
		protected override bool IsValid(GameState state, Config config) {
			if ( state.Level == null ) {
				return false;
			}
			return true;
		}

		protected override void Execute(GameState state, Config config, ICommandBuffer buffer) {
			foreach ( var unit in state.Level.PlayerUnits ) {
				state.AddUnit(unit);
			}
			var reward = config.Levels[state.Level.Descriptor].Reward;
			state.Level = null;

			if ( !Win ) {
				return;
			}
			
			foreach ( var pair in reward.Resources ) {
				buffer.AddCommand(new AddResourceCommand(pair.Key, pair.Value));
			}
			foreach ( var itemDesc in reward.Items ) {
				buffer.AddCommand(new AddItemCommand(state.NewEntityId(), itemDesc));
			}
			foreach ( var unitDesc in reward.Units ) {
				buffer.AddCommand(new AddUnitCommand(state.NewEntityId(), unitDesc, 1));
			}
		}

		public override string ToString() {
			return $"{nameof(FinishLevelCommand)} ({Win})";
		}
	}
}