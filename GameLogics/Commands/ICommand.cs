using GameLogics.Core;

namespace GameLogics.Commands {
	public interface ICommand {
		void Execute(GameState state);
	}
}