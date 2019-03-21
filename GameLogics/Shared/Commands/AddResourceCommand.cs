using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Commands {
	public sealed class AddResourceCommand : InternalCommand {
		public readonly Resource Kind;
		public readonly int      Count;

		public AddResourceCommand(Resource kind, int count) {
			Kind  = kind;
			Count = count;
		}

		protected override bool IsValid(GameState state, Config config) => (Kind != Resource.Unknown) && (Count > 0);

		protected override void Execute(GameState state, Config config, ICommandBuffer _) {
			var oldValue = state.Resources.GetOrDefault(Kind);
			state.Resources[Kind] = oldValue + Count;
		}
		
		public override string ToString() {
			return $"{nameof(AddResourceCommand)} ({Kind}, {Count})";
		}
	}
}