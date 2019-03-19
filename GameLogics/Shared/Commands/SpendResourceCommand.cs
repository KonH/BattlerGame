using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Commands {
	public sealed class SpendResourceCommand : BaseCommand {
		public readonly Resource Kind;
		public readonly int      Count;

		public SpendResourceCommand(Resource kind, int count) {
			Kind  = kind;
			Count = count;
		}

		public override bool IsValid(GameState state, Config config) => (Kind != Resource.Unknown) && (Count > 0);

		protected override void ExecuteSingle(GameState state, Config config) {
			var oldValue = state.Resources.GetOrDefault(Kind);
			state.Resources[Kind] = oldValue - Count;
		}
		
		public override string ToString() {
			return $"{nameof(SpendResourceCommand)} ({Kind}, {Count})";
		}
	}
}