using GameLogics.Shared.Model;

namespace UnityClient.Model {
	public abstract class ItemModel {
		public ItemType Type { get; protected set; }
		public string   Name { get; protected set; } = null;
		
		public bool   HasAction  => OnClick != null;
		public string ActionName => OnClick?.Name;
		public bool Interactable => HasAction && OnClick.Interactable;

		protected ClickAction<ItemModel> OnClick { get; set; } = null;

		public void Click() {
			OnClick?.Callback.Invoke(this);
		}
	}
}