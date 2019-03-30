using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace UnityClient.Models {
	public class UnitModel {
		public bool       IsPlayerUnit { get; }
		public UnitState  State        { get; }
		public UnitConfig Config       { get; }

		public UnitModel(bool isPlayerUnit, UnitState state, UnitConfig config) {
			IsPlayerUnit = isPlayerUnit;
			State        = state;
			Config       = config;
		}
	}
}