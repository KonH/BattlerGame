using System.Threading.Tasks;
using GameLogics.Client.Services;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using UnityClient.Models;
using UnityClient.Services;
using UnityClient.ViewModels;
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
		UnitViewModel.Factory  _units;
		
		[Inject]
		public void Init(GameSceneManager scene, GameStateUpdateService update, ClientStateService state, NoticeService notice, UnitViewModel.Factory units) {
			_scene  = scene;
			_update = update;
			_state  = state;
			_notice = notice;
			_units  = units;
			
			_update.AddHandler<FinishLevelCommand>(OnFinishLevel);
		}

		void OnDestroy() {
			_update.RemoveHandler<FinishLevelCommand>(OnFinishLevel);
		}

		Task OnFinishLevel(ICommand c) {
			_notice.ScheduleNotice(new NoticeModel("You won!", _ => _scene.GoToWorld()));
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
			var model = new UnitLevelModel(isPlayerUnit, state, config);
			var instance = _units.Create(model);
			instance.transform.SetParent(points[position], false);
		}
	}
}