using GameLogics.Shared.Command.Base;
using GameLogics.Shared.Model;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Command {
	public sealed class SpendResourceCommand : ICommand {
		public readonly Resource Kind;
		public readonly int      Count;

		public SpendResourceCommand(Resource kind, int count) {
			Kind  = kind;
			Count = count;
		}

		public bool IsValid(GameState state, ConfigRoot config) => (Kind != Resource.Unknown) && (Count > 0);

		public void Execute(GameState state, ConfigRoot config, ICommandBuffer _) {
			var oldValue = state.Resources.GetOrDefault(Kind);
			state.Resources[Kind] = oldValue - Count;
		}
		
		public override string ToString() {
			return $"{nameof(SpendResourceCommand)} ({Kind}, {Count})";
		}
	}
}