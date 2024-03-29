using GameLogics.Shared.Command.Base;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Command {
	public sealed class TakeOffItemCommand : ICommand {
		public readonly ulong ItemId;
		public readonly ulong UnitId;

		public TakeOffItemCommand(ulong itemId, ulong unitId) {
			ItemId = itemId;
			UnitId = unitId;
		}
		
		public bool IsValid(GameState state, ConfigRoot config) {
			var unit = FindUnit(state);
			if ( unit == null ) {
				return false;
			}
			return (FindItem(unit) != null);
		}

		public void Execute(GameState state, ConfigRoot config, ICommandBuffer _) {
			var unit = FindUnit(state);
			var item = FindItem(unit);
			unit.Items.Remove(item);
			
			state.Items.Add(item.Id, item);
		}

		UnitState FindUnit(GameState state) => state.Units.GetOrDefault(UnitId);

		ItemState FindItem(UnitState state) => state.Items.Find(it => it.Id == ItemId);

		public override string ToString() {
			return $"{nameof(TakeOffItemCommand)} ({ItemId}, {UnitId})";
		}
	}
}