using System.Collections.Generic;
using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Commands {
	public class StartLevelCommand : BaseCommand {
		public readonly string      LevelDesc;
		public readonly List<ulong> PlayerUnits;

		public StartLevelCommand(string levelDesc, List<ulong> playerUnits) {
			LevelDesc   = levelDesc;
			PlayerUnits = playerUnits;
		}
		
		protected override bool IsValid(GameState state, Config config) {
			if ( string.IsNullOrEmpty(LevelDesc) ) {
				return false;
			}
			if ( !config.Levels.ContainsKey(LevelDesc) ) {
				return false;
			}
			if ( (PlayerUnits == null) || (PlayerUnits.Count == 0) ) {
				return false;
			}
			var playerUnits = FindPlayerUnits(state);
			if ( playerUnits == null ) {
				return false;
			}
			foreach ( var unit in playerUnits ) {
				if ( unit.Health <= 0 ) {
					return false;
				}
			}
			return true;
		}

		protected override void Execute(GameState state, Config config, ICommandBuffer _) {
			var playerUnits = FindPlayerUnits(state);
			var enemyUnits = CreateEnemyUnits(state, config);
			
			state.Level = new LevelState(LevelDesc, playerUnits, enemyUnits);
			state.Level.PlayerTurn = true;
			
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

		List<UnitState> CreateEnemyUnits(GameState state, Config config) {
			var levelConfig = FindLevelConfig(config);
			var enemyUnits  = new List<UnitState>();
			foreach ( var enemyDesc in levelConfig.EnemyDescriptors ) {
				enemyUnits.Add(new UnitState(enemyDesc, 1).WithId(state.NewEntityId()));
			}
			return enemyUnits;
		}
		
		public override string ToString() {
			return $"{nameof(StartLevelCommand)} ('{LevelDesc}', {string.Join(", ", PlayerUnits)})";
		}
	}
}