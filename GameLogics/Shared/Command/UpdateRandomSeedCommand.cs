using GameLogics.Shared.Command.Base;
using GameLogics.Shared.Model.Config;
using GameLogics.Shared.Model.State;

namespace GameLogics.Shared.Command {
	public sealed class UpdateRandomSeedCommand : IInternalCommand {
		public bool IsValid(GameState state, ConfigRoot config) => true;

		public void Execute(GameState state, ConfigRoot config, ICommandBuffer buffer) {
			state.Random.Seed++;
		}

		public override string ToString() {
			return $"{nameof(UpdateRandomSeedCommand)}";
		}
	}
}