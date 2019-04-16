using System;
using GameLogics.Shared.Command.Base;
using GameLogics.Shared.Model.Config;
using GameLogics.Shared.Model.State;

namespace GameLogics.Shared.Command {
	public sealed class AddPersistentTimeOffsetCommand : IDebugCommand {
		public readonly TimeSpan Amount;

		public AddPersistentTimeOffsetCommand(TimeSpan amount) {
			Amount = amount;
		}

		public bool IsValid(GameState state, ConfigRoot config) => true;

		public void Execute(GameState state, ConfigRoot config, ICommandBuffer buffer) {
			state.Time.PersistentOffset += Amount;
		}

		public override string ToString() {
			return $"{nameof(AddPersistentTimeOffsetCommand)} ({Amount})";
		}
	}
}
