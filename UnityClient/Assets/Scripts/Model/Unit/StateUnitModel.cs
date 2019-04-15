using GameLogics.Shared.Model.State;

namespace UnityClient.Model {
	public sealed class StateUnitModel : UnitModel {
		public UnitState State { get; }
		
		public StateUnitModel(UnitState state, int index = -1, ClickAction<UnitModel> onClick = null) {
			Name    = $"{state.Id} ({state.Descriptor})";
			OnClick = onClick;
			State   = state;
			Index   = index;
		}
	}
}