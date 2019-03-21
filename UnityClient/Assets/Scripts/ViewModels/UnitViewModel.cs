using GameLogics.Client.Services;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Commands.Base;
using UnityClient.Models;
using UnityClient.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class UnitViewModel : MonoBehaviour, IPointerClickHandler {
	public class Factory : PlaceholderFactory<UnitLevelModel, UnitViewModel> {}

	GameStateUpdateService _updateService;
	LevelService           _levelService;
	UnitLevelModel         _model;

	[Inject]
	public void Init(GameStateUpdateService updateService, LevelService levelService, UnitLevelModel model) {
		_updateService = updateService;
		_levelService  = levelService;
		_model         = model;
		
		_updateService.OnCommandApplied += OnCommandApplied;
	}

	void OnDestroy() {
		_updateService.OnCommandApplied -= OnCommandApplied;
	}

	void OnCommandApplied(ICommand obj) {
		if ( obj is KillUnitCommand cmd ) {
			if ( cmd.UnitId == _model.State.Id ) {
				gameObject.SetActive(false);
			}
		}
	}

	public void OnPointerClick(PointerEventData eventData) {
		if ( _model.IsPlayerUnit ) {
			_levelService.SelectUnit(_model.State.Id);
		} else {
			// TODO: Handle async
			_levelService.AttackUnit(_model.State.Id);
		}
	}
}
