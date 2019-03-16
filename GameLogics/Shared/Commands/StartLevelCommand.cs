using System.Collections.Generic;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Commands {
	public class StartLevelCommand : ICommand {
		public readonly string       LevelDesc;
		public readonly List<string> PlayerUnits;

		public StartLevelCommand(string levelDesc, List<string> playerUnits) {
			LevelDesc   = levelDesc;
			PlayerUnits = playerUnits;
		}
		
		public bool IsValid(GameState state, Config config) {
			if ( string.IsNullOrEmpty(LevelDesc) ) {
				return false;
			}
			if ( (PlayerUnits == null) || (PlayerUnits.Count == 0) ) {
				return false;
			}
			return config.Levels.ContainsKey(LevelDesc) && (FindPlayerUnits(state) != null);
		}

		public void Execute(GameState state, Config config) {
			var playerUnits = FindPlayerUnits(state);
			var enemyUnits = GetEnemyUnits(config);
			
			state.Level = new LevelState(LevelDesc, playerUnits, enemyUnits);
			
			foreach ( var unitId in PlayerUnits ) {
				state.Units.Remove(unitId);
			}
		}

		LevelConfig FindLevelConfig(Config config) => config.Levels.GetOrDefault(LevelDesc);

		List<UnitState> FindPlayerUnits(GameState state) {
			var playerUnits = new List<UnitState>();
			foreach ( var unitId in PlayerUnits ) {
				var unit = state.Units.GetOrDefault(unitId);
				if ( unit == null ) {
					return null;
				}
				playerUnits.Add(unit);
			}
			return playerUnits;
		}

		List<UnitState> GetEnemyUnits(Config config) {
			var levelConfig = FindLevelConfig(config);
			var enemyUnits  = new List<UnitState>();
			for ( var i = 0; i < levelConfig.EnemyDescriptors.Count; i++ ) {
				var enemyDesc = levelConfig.EnemyDescriptors[i];
				enemyUnits.Add(new UnitState(enemyDesc, 1).WithId(i.ToString()));
			}
			return enemyUnits;
		}
		
		public override string ToString() {
			return $"{nameof(StartLevelCommand)} ('{LevelDesc}', {string.Join(", ", PlayerUnits)})";
		}
	}
}