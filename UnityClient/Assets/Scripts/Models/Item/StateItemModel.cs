using GameLogics.Shared.Models.State;
using GameLogics.Shared.Models.Configs;

namespace UnityClient.Models {
	public sealed class StateItemModel : ItemModel {
		public ItemState State { get; }
		
		public StateItemModel(ItemState state, IItemConfig config, ClickAction<ItemModel> onClick) {
			Type    = config.Type;
			Name    = $"{state.Descriptor} ({config.Type})";
			OnClick = onClick;
			State   = state;
		}
	}
}