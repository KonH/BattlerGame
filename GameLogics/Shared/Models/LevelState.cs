using System.Collections.Generic;

namespace GameLogics.Shared.Models {
	public class LevelState {
		public string          Descriptor  { get; set; }
		public List<UnitState> PlayerUnits { get; set; }
		public List<UnitState> EnemyUnits  { get; set; }

		public LevelState(string descriptor, List<UnitState> playerUnits, List<UnitState> enemyUnits) {
			Descriptor  = descriptor;
			PlayerUnits = playerUnits;
			EnemyUnits  = enemyUnits;
		}

		public UnitState FindUnitById(string id) {
			foreach ( var unit in PlayerUnits ) {
				if ( unit.Id == id ) {
					return unit;
				}
			}
			foreach ( var unit in EnemyUnits ) {
				if ( unit.Id == id ) {
					return unit;
				}
			}
			return null;
		}
	}
}