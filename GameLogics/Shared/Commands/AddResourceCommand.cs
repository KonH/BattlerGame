using System;
using GameLogics.Shared.Models;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Commands {
	public sealed class AddResourceCommand : ICommand {
		public readonly Resource Kind;
		public readonly int      Count;

		public AddResourceCommand(Resource kind, int count) {
			Kind  = (kind != Resource.Unknown) ? kind : throw new InvalidOperationException(nameof(kind));
			Count = count;
		}

		public void Execute(GameState state) {
			var oldValue = state.Resources.GetOrDefault(Kind);
			state.Resources[Kind] = oldValue + Count;
		}
		
		public override string ToString() {
			return string.Format("{0} ({1}, {2})", nameof(AddResourceCommand), Kind, Count);
		}
	}
}