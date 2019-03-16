using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Commands {
	public sealed class AddResourceCommand : ICommand {
		public readonly Resource Kind;
		public readonly int      Count;

		public AddResourceCommand(Resource kind, int count) {
			Kind  = kind;
			Count = count;
		}

		public bool IsValid(GameState state, Config config) => (Kind != Resource.Unknown) && (Count > 0);

		public void Execute(GameState state, Config config) {
			var oldValue = state.Resources.GetOrDefault(Kind);
			state.Resources[Kind] = oldValue + Count;
		}
		
		public override string ToString() {
			return string.Format("{0} ({1}, {2})", nameof(AddResourceCommand), Kind, Count);
		}
	}
}