using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;

namespace GameLogics.Shared.Command.Base {
	public interface ICommand {
		bool IsValid(GameState state, ConfigRoot config);
		void Execute(GameState state, ConfigRoot config, ICommandBuffer buffer);
	}
}