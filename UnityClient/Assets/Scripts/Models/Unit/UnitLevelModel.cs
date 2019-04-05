using GameLogics.Shared.Models.State;
using GameLogics.Shared.Models.Configs;

namespace UnityClient.Models {
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