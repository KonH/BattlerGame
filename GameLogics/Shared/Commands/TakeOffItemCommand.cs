using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Commands {
	public class TakeOffItemCommand : BaseCommand {
		public readonly string ItemId;
		public readonly string UnitId;

		public TakeOffItemCommand(string itemId, string unitId) {
			ItemId = itemId;
			UnitId = unitId;
		}
		
		public override bool IsValid(GameState state, Config config) {
			if ( string.IsNullOrEmpty(ItemId) || string.IsNullOrEmpty(UnitId) ) {
				return false;
			}
			var unit = FindUnit(state);
			if ( unit == null ) {
				return false;
			}
			return (FindItem(unit) != null);
		}

		public override void Execute(GameState state, Config config) {
			var unit = FindUnit(state);
			var item = FindItem(unit);
			unit.Items.Remove(item);
			
			state.Items.Add(item.Id, item);
		}

		UnitState FindUnit(GameState state) => state.Units.GetOrDefault(UnitId);

		ItemState FindItem(UnitState state) => state.Items.Find(it => it.Id == ItemId);

		public override string ToString() {
			return $"{nameof(TakeOffItemCommand)} ()";
		}
	}
}