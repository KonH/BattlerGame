using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Models.State;

namespace GameLogics.Shared.Commands {
	public sealed class UpdateRandomSeedCommand : IInternalCommand {
		public bool IsValid(GameState state, Config config) => true;

		public void Execute(GameState state, Config config, ICommandBuffer buffer) {
			state.Random.Seed++;
		}

		public override string ToString() {
			return $"{nameof(UpdateRandomSeedCommand)}";
		}
	}
}