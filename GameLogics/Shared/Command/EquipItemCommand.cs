using GameLogics.Shared.Command.Base;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Command {
	public sealed class EquipItemCommand : ICommand {
		public readonly ulong ItemId;
		public readonly ulong UnitId;

		public EquipItemCommand(ulong itemId, ulong unitId) {
			ItemId = itemId;
			UnitId = unitId;
		}
		
		public bool IsValid(GameState state, ConfigRoot config) {
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

		public void Execute(GameState state, ConfigRoot config, ICommandBuffer _) {
			var item = FindItem(state);
			var unit = FindUnit(state);
			unit.Items.Add(item);

			state.Items.Remove(item.Id);
		}

		UnitState FindUnit(GameState state) => state.Units.GetOrDefault(UnitId);

		ItemState FindItem(GameState state) => state.Items.GetOrDefault(ItemId);

		public override string ToString() {
			return $"{nameof(EquipItemCommand)} ({ItemId}, {UnitId})";
		}
	}
}