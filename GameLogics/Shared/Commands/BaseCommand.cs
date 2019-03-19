using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands {
	public abstract class BaseCommand : ICommand {
		public abstract bool IsValid(GameState state, Config config);
		public abstract void Execute(GameState state, Config config);

		public bool TryExecute(GameState state, Config config) {
			if ( IsValid(state, config) ) {
				Execute(state, config);
				return true;
			}
			return false;
		}
	}
}