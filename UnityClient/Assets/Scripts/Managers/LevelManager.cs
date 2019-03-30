using System.Threading.Tasks;
using GameLogics.Client.Services;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using UnityClient.Models;
using UnityClient.Services;
using UnityClient.ViewModels;
using UnityClient.ViewModels.Windows;
using UnityEngine;
using Zenject;

namespace UnityClient.Managers {
	public class LevelManager : MonoBehaviour, IInitializable {
		public Transform[] PlayerPoints = null;
		public Transform[] EnemyPoints  = null;

		GameSceneManager       _scene;
		GameStateUpdateService _update;
		ClientStateService     _state;
		NoticeService          _notice;
		LevelService           _service;
		UnitViewModel.Factory  _units;
		UiManager              _ui;
		
		[Inject]
		public void Init(
			GameSceneManager scene, GameStateUpdateService update, ClientStateService state, NoticeService notice,
			LevelService service, UnitViewModel.Factory units, UiManager ui
		) {
			_scene   = scene;
			_update  = update;
			_state   = state;
			_notice  = notice;
			_service = service;
			_units   = units;
			_ui      = ui;
			
			_update.AddHandler<EndPlayerTurnCommand>(OnEndPlayerTurn);
			_update.AddHandler<EndEnemyTurnCommand> (OnEndEnemyTurn);
			_update.AddHandler<FinishLevelCommand>  (OnFinishLevel);
		}

		void OnDestroy() {
			_update.RemoveHandler<EndPlayerTurnCommand>(OnEndPlayerTurn);
			_update.RemoveHandler<EndEnemyTurnCommand> (OnEndEnemyTurn);
			_update.RemoveHandler<FinishLevelCommand>  (OnFinishLevel);
		}

		Task OnEndPlayerTurn(EndPlayerTurnCommand _) {
			_service.OnFinishPlayerTurn();
			return Task.CompletedTask;
		}

		Task OnEndEnemyTurn(EndEnemyTurnCommand _) {
			_service.OnFinishEnemyTurn();
			return Task.CompletedTask;
		}

		Task OnFinishLevel(FinishLevelCommand cmd) {
			if ( cmd.Win ) {
				_notice.ScheduleNotice(new NoticeModel("You won!", _ => _scene.GoToWorld()));
			} else {
				_ui.ShowWindow<LoseWindow>(w => w.Show(_scene.GoToWorld));
			}
			return Task.CompletedTask;
		}

		public void Initialize() {
			var state = _state.State?.Level;
			if ( state == null ) {
				return;
			}
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
			return _state.Config.Units[state.Descriptor];
		}

		void AddUnit(bool isPlayerUnit, UnitState state, UnitConfig config, Transform[] points, int position) {
			var model = new UnitModel(isPlayerUnit, state, config);
			var instance = _units.Create(model);
			instance.transform.SetParent(points[position], false);
		}
	}
}