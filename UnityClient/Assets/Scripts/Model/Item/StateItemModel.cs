using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;

namespace UnityClient.Model {
	public sealed class StateItemModel : ItemModel {
		public ItemState State { get; }
		
		public StateItemModel(ItemState state, BaseItemConfig config, ClickAction<ItemModel> onClick) {
			Type    = config.Type;
			Name    = $"{state.Descriptor}, lvl.{state.Level + 1} ({config.Type}, +{GetItemValue(state, config)})";
			OnClick = onClick;
			State   = state;
		}

		string GetItemValue(ItemState state, BaseItemConfig config) {
			switch ( config ) {
				case WeaponConfig w: return w.Damage[state.Level].ToString();
				case ArmorConfig a:  return a.Absorb[state.Level].ToString();
				default:             return "?";
			}
		}
	}
}