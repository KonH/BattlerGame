namespace UnityClient.Models {
	public sealed class PlaceholderUnitModel : UnitModel {
		public PlaceholderUnitModel(int index, ClickAction<UnitModel> onClick) {
			Name = $"Unit {index + 1}";
			Index = index;
			OnClick = onClick;
		}
	}
}