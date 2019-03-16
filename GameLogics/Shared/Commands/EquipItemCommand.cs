using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Commands {
	public class EquipItemCommand : ICommand {
		public readonly string ItemId;
		public readonly string UnitId;

		public EquipItemCommand(string itemId, string unitId) {
			ItemId = itemId;
			UnitId = unitId;
		}
		
		public bool IsValid(GameState state, Config config) {
			if ( string.IsNullOrEmpty(ItemId) || string.IsNullOrEmpty(UnitId) ) {
				return false;
			}
			var item = FindItem(state);
			if ( item == null ) {
				return false;
			}
			var itemDesc = config.Items.GetOrDefault(item.Descriptor);
			if ( itemDesc == null ) {
				return false;
			}
			var unit = FindUnit(state);
			if ( unit == null ) {
				return false;
			}
			var unitDesc = config.Units.GetOrDefault(unit.Descriptor);
			if ( unitDesc == null ) {
				return false;
			}
			var itemWithSameType = unit.Items.Find(it => {
				var curDesc = config.Items.GetOrDefault(it.Descriptor);
				return (curDesc != null) && (curDesc.Type == itemDesc.Type);
			});
			return (itemWithSameType == null);
		}

		public void Execute(GameState state, Config config) {
			var item = FindItem(state);
			var unit = FindUnit(state);
			unit.Items.Add(item);

			state.Items.Remove(item.Id);
		}

		UnitState FindUnit(GameState state) => state.Units.GetOrDefault(UnitId);

		ItemState FindItem(GameState state) => state.Items.GetOrDefault(ItemId);

		public override string ToString() {
			return $"{nameof(EquipItemCommand)} ()";
		}
	}
}