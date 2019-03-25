using System.Threading.Tasks;
using GameLogics.Client.Services;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Commands.Base;
using UnityClient.Models;
using UnityClient.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UnityClient.ViewModels {
	public class UnitViewModel : MonoBehaviour, IPointerClickHandler {
		public class Factory : PlaceholderFactory<UnitLevelModel, UnitViewModel> {}

		public GameObject Selection = null;
		
		GameStateUpdateService _updateService;
		LevelService           _levelService;
		UnitLevelModel         _model;

		[Inject]
		public void Init(GameStateUpdateService updateService, LevelService levelService, UnitLevelModel model) {
			_updateService = updateService;
			_levelService  = levelService;
			_model         = model;

			_levelService.OnUnitSelected += OnUnitSelected;
			_updateService.AddHandler<KillUnitCommand>(OnKillUnit);

			UpdateSelection(false);
		}

		void OnDestroy() {
			_levelService.OnUnitSelected -= OnUnitSelected;
			_updateService?.RemoveHandler<KillUnitCommand>(OnKillUnit);
		}

		void OnUnitSelected(ulong id) {
			UpdateSelection(id == _model.State.Id);
		}

		void UpdateSelection(bool active) {
			Selection.SetActive(active);
		}
		
		Task OnKillUnit(ICommand c) {
			var cmd = c as KillUnitCommand;
			if ( cmd?.UnitId == _model.State.Id ) {
				gameObject.SetActive(false);
			}
			return Task.CompletedTask;
		}

		public void OnPointerClick(PointerEventData eventData) {
			if ( _model.IsPlayerUnit ) {
				_levelService.SelectUnit(_model.State.Id);
			} else {
				_levelService.AttackUnit(_model.State.Id);
			}
		}
	}
}
