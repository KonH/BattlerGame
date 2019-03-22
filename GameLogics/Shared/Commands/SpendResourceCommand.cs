using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Commands {
	public sealed class SpendResourceCommand : ICommand {
		public readonly Resource Kind;
		public readonly int      Count;

		public SpendResourceCommand(Resource kind, int count) {
			Kind  = kind;
			Count = count;
		}

		public bool IsValid(GameState state, Config config) => (Kind != Resource.Unknown) && (Count > 0);

		public void Execute(GameState state, Config config, ICommandBuffer _) {
			var oldValue = state.Resources.GetOrDefault(Kind);
			state.Resources[Kind] = oldValue - Count;
		}
		
		public override string ToString() {
			return $"{nameof(SpendResourceCommand)} ({Kind}, {Count})";
		}
	}
}