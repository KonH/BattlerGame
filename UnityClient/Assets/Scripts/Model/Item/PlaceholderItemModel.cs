using GameLogics.Shared.Model;

namespace UnityClient.Model {
	public sealed class PlaceholderItemModel : ItemModel {
		public PlaceholderItemModel(ItemType type, ClickAction<ItemModel> onClick) {
			Type    = type;
			Name    = type.ToString();
			OnClick = onClick;
		}
	}
}