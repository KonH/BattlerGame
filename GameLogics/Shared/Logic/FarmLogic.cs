using GameLogics.Shared.Model.Config;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Logic {
	public static class FarmLogic {
		public static bool IsFarmigLevel(string desc, ConfigRoot config) {
			var scope = LevelUtils.GetScope(desc);
			return config.Farming.ContainsKey(scope);
		}

		public static bool IsAvailable(string name, GameState state, ConfigRoot config) {
			if ( !config.Farming.TryGetValue(name, out var info) ) {
				return false;
			}
			var time = state.Time.GetRealTime();
			var lastTime = state.Farming.GetOrDefault(name);
			return (time >= lastTime.Add(info.Interval));
		}

		public static (string name, FarmConfig config) GetFirstAvailable(GameState state, ConfigRoot config) {
			foreach ( var pair in config.Farming ) {
				if ( IsAvailable(pair.Key, state, config) ) {
					return (pair.Key, pair.Value);
				}
			}
			return (string.Empty, null);
		}
	}
}
