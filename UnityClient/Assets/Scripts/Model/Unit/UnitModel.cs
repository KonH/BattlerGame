namespace UnityClient.Model {
	public abstract class UnitModel {
		public string Name  { get; protected set; } = null;
		public int    Index { get; protected set; } = -1;
		
		public bool   HasAction  => OnClick != null;
		public string ActionName => OnClick?.Name;
		
		protected ClickAction<UnitModel> OnClick { get; set; } = null;

		public void Click() {
			OnClick?.Callback.Invoke(this);
		}
	}
}