using GameLogics.Shared.Models.State;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands.Base {
	public interface ICommand {
		bool IsValid(GameState state, Config config);
		void Execute(GameState state, Config config, ICommandBuffer buffer);
	}
}