using GameLogics.Client.Services;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using UnityClient.Models;
using UnityEngine;
using Zenject;

namespace UnityClient.Managers {
	public class LevelManager : MonoBehaviour, IInitializable {
		public Transform[] PlayerPoints = null;
		public Transform[] EnemyPoints  = null;

		GameSceneManager       _sceneManager;
		GameStateUpdateService _updateService;
		ClientStateService     _stateService;
		UnitViewModel.Factory  _unitFactory;
		
		[Inject]
		public void Init(GameSceneManager sceneManager, GameStateUpdateService updateService, ClientStateService stateService, UnitViewModel.Factory unitFactory) {
			_sceneManager  = sceneManager;
			_updateService = updateService;
			_stateService  = stateService;
			_unitFactory   = unitFactory;
			
			_updateService.OnCommandApplied += OnCommandApplied;
		}

		void OnDestroy() {
			_updateService.OnCommandApplied -= OnCommandApplied;
		}

		void OnCommandApplied(ICommand obj) {
			if ( obj is FinishLevelCommand _ ) {
				_sceneManager.GoToWorld();
			}
		}

		public void Initialize() {
			var state = _stateService.State.Level;
			var playerUnits = state.PlayerUnits;
			for ( var i = 0; i < playerUnits.Count; i++ ) {
				var unit = playerUnits[i];
				AddUnit(true, unit, GetUnitConfig(unit), PlayerPoints, i);
			}
			var enemyUnits = state.EnemyUnits;
			for ( var i = 0; i < enemyUnits.Count; i++ ) {
				var unit = enemyUnits[i];
				AddUnit(false, unit, GetUnitConfig(unit), EnemyPoints, i);
			}
		}

		UnitConfig GetUnitConfig(UnitState state) {
			return _stateService.Config.Units[state.Descriptor];
		}

		void AddUnit(bool isPlayerUnit, UnitState state, UnitConfig config, Transform[] points, int position) {
			var model = new UnitLevelModel(isPlayerUnit, state, config);
			var instance = _unitFactory.Create(model);
			instance.transform.SetParent(points[position], false);
		}
	}
}