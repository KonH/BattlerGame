using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;

namespace UnityClient.Model {
	public sealed class UnitLevelModel {
		public bool       IsPlayerUnit { get; }
		public UnitState  State        { get; }
		public UnitConfig Config       { get; }

		public UnitLevelModel(bool isPlayerUnit, UnitState state, UnitConfig config) {
			IsPlayerUnit = isPlayerUnit;
			State        = state;
			Config       = config;
		}
	}
}