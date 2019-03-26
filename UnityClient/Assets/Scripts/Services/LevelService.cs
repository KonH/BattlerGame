using System;
using GameLogics.Shared.Commands;

namespace UnityClient.Services {
	public class LevelService {
		public event Action<ulong>        OnUnitSelected = delegate {};
		public event Action<ulong?, bool> OnUnitCanTurn  = delegate {};
		
		readonly ClientCommandRunner _runner;

		ulong _selectedUnitId = 0;
		
		public LevelService(ClientCommandRunner runner) {
			_runner = runner;
		}
		
		public void SelectUnit(ulong unitId) {
			_selectedUnitId = unitId;
			OnUnitSelected(unitId);
		}
		
		public void AttackUnit(ulong unitId) {
			if ( _selectedUnitId == 0 ) {
				return;
			}
			if ( _runner.TryAddCommand(new AttackCommand(_selectedUnitId, unitId)) ) {
				OnUnitCanTurn(_selectedUnitId, false);
				_selectedUnitId = 0;
			}
		}

		public void OnFinishPlayerTurn() {
			OnUnitCanTurn(null, false);
		}

		public void OnFinishEnemyTurn() {
			OnUnitCanTurn(null, true);
		}
	}
}