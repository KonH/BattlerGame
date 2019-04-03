using GameLogics.Shared.Models;

namespace UnityClient.Models {
	public abstract class ItemModel {
		public ItemType Type { get; protected set; }
		public string   Name { get; protected set; } = null;
		
		public bool   HasAction  => OnClick != null;
		public string ActionName => OnClick?.Name;

		protected ClickAction<ItemModel> OnClick { get; set; } = null;

		public void Click() {
			OnClick?.Callback.Invoke(this);
		}
	}
}