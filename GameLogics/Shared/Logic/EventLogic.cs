using GameLogics.Shared.Model.Config;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Logic {
	public static class EventLogic {
		public static bool IsActive(string name, GameState state, ConfigRoot config) {
			if ( !config.Events.TryGetValue(name, out var info) ) {
				return false;
			}
			if ( state.Events.ContainsKey(name) ) {
				return false;
			}
			var time = state.Time.GetRealTime();
			return (time >= info.StartTime) && (time <= info.StartTime.Add(info.Duration));
		}

		public static bool IsCompleted(string name, GameState state, ConfigRoot config) {
			if ( !IsActive(name, state, config) ) {
				return false;
			}
			var scope = config.Events[name].Scope;
			var progress = state.Progress.GetOrDefault(scope);
			var nextLevelDesc = LevelUtils.GetDesc(scope, progress);
			return !config.Levels.ContainsKey(nextLevelDesc);
		}

		public static (string name, EventConfig config) GetFirstActive(GameState state, ConfigRoot config) {
			foreach ( var pair in config.Events ) {
				if ( IsActive(pair.Key, state, config) ) {
					return (pair.Key, pair.Value);
				}
			}
			return (string.Empty, null);
		}
	}
}
