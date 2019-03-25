using System.Threading.Tasks;
using GameLogics.Client.Services;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models;
using UnityClient.Models;
using UnityClient.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.ViewModels {
	public class UnitViewModel : MonoBehaviour, IPointerClickHandler {
		public class Factory : PlaceholderFactory<UnitLevelModel, UnitViewModel> {}

		public Color PlayerHealthColor = Color.green;
		public Color EnemyHealthColor  = Color.red;
		
		public GameObject Selection    = null;
		public Slider     HealthSlider = null;
		public Image      HealthImage  = null;
		
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
			_updateService.OnStateUpdated += OnStateUpdated;

			HealthImage.color = model.IsPlayerUnit ? PlayerHealthColor : EnemyHealthColor;
			UpdateSelection(false);
			UpdateHealth();
		}

		void OnDestroy() {
			_levelService.OnUnitSelected -= OnUnitSelected;
			_updateService.RemoveHandler<KillUnitCommand>(OnKillUnit);
			_updateService.OnStateUpdated -= OnStateUpdated;
		}

		void OnUnitSelected(ulong id) {
			UpdateSelection(id == _model.State.Id);
		}

		void UpdateHealth() {
			HealthSlider.value = (float)_model.State.Health / _model.Config.MaxHealth;
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

		void OnStateUpdated(GameState state) {
			UpdateHealth();
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
