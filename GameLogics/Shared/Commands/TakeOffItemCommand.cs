using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Commands {
	public class TakeOffItemCommand : BaseCommand {
		public readonly ulong ItemId;
		public readonly ulong UnitId;

		public TakeOffItemCommand(ulong itemId, ulong unitId) {
			ItemId = itemId;
			UnitId = unitId;
		}
		
		public override bool IsValid(GameState state, Config config) {
			var unit = FindUnit(state);
			if ( unit == null ) {
				return false;
			}
			return (FindItem(unit) != null);
		}

		protected override void ExecuteSingle(GameState state, Config config) {
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