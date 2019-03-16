using GameLogics.Shared.Models;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Commands {
	public sealed class SpendResourceCommand : ICommand {
		public readonly Resource Kind;
		public readonly int      Count;

		public SpendResourceCommand(Resource kind, int count) {
			Kind  = kind;
			Count = count;
		}

		public bool IsValid(GameState state) => (Kind != Resource.Unknown) && (Count > 0);

		public void Execute(GameState state) {
			var oldValue = state.Resources.GetOrDefault(Kind);
			state.Resources[Kind] = oldValue - Count;
		}
		
		public override string ToString() {
			return string.Format("{0} ({1}, {2})", nameof(SpendResourceCommand), Kind, Count);
		}
	}
}