using GameLogics.Client.Services;
using GameLogics.Shared.Models;
using UnityEngine;
using Zenject;

namespace UnityClient.Managers {
	public class LevelManager : MonoBehaviour, IInitializable {
		public Transform[] PlayerPoints = null;
		public Transform[] EnemyPoints  = null;
		
		LevelState            _state;
		UnitViewModel.Factory _unitFactory;
		
		[Inject]
		public void Init(ClientStateService service, UnitViewModel.Factory unitFactory) {
			_state       = service.State.Level;
			_unitFactory = unitFactory;
		}

		public void Initialize() {
			var playerUnits = _state.PlayerUnits;
			for ( var i = 0; i < playerUnits.Count; i++ ) {
				AddUnit(playerUnits[i], PlayerPoints, i);
			}
			var enemyUnits = _state.PlayerUnits;
			for ( var i = 0; i < enemyUnits.Count; i++ ) {
				AddUnit(enemyUnits[i], EnemyPoints, i);
			}
		}

		void AddUnit(UnitState unit, Transform[] points, int position) {
			var instance = _unitFactory.Create();
			instance.transform.SetParent(points[position], false);
		}
	}
}