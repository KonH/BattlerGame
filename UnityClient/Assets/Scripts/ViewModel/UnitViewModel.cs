using System.Threading.Tasks;
using GameLogics.Client.Service;
using GameLogics.Shared.Command;
using GameLogics.Shared.Model.State;
using UnityClient.Model;
using UnityClient.Service;
using UnityClient.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityClient.View;
using Zenject;

namespace UnityClient.ViewModel {
	public sealed class UnitViewModel : MonoBehaviour, IPointerClickHandler {
		public sealed class Factory : PlaceholderFactory<Transform, UnitLevelModel, UnitView, UnitViewModel> {}

		public Transform  ViewRoot     = null;
		public GameObject Selection    = null;
		public GameObject Interactable = null;
		
		GameStateUpdateService _updateService;
		LevelService           _levelService;
		UnitLevelModel         _model;
		UnitView               _view;

		int _oldHealth;

		[Inject]
		public void Init(GameStateUpdateService updateService, LevelService levelService, Transform parent, UnitLevelModel model, UnitView view, Canvas canvas) {
			_updateService = updateService;
			_levelService  = levelService;
			_model         = model;
			_view          = view;
			_oldHealth     = _model.State.Health;

			_levelService.OnUnitSelected += OnUnitSelected;
			_levelService.OnUnitCanTurn  += OnUnitCanTurn;
			_updateService.OnStateUpdated += OnStateUpdated;
			_updateService.AddHandler<AttackCommand>(OnAttackUnit);
			_updateService.AddHandler<KillUnitCommand>(OnKillUnit);

			SelectView();
			UpdateSelection(false);
			UpdateInteractable(model.IsPlayerUnit);
			UpdateHealth();

			transform.SetParent(parent, false);

			_view.transform.SetParent(canvas.transform, false);
			_view.transform.position = Camera.main.WorldToScreenPoint(transform.position);
			var distance = transform.position.z - Camera.main.transform.position.z;
			_view.UpdateDistance(distance);
			_view.SelectColor(model.IsPlayerUnit);
		}

		void OnDestroy() {
			_levelService.OnUnitSelected -= OnUnitSelected;
			_levelService.OnUnitCanTurn  -= OnUnitCanTurn;
			_updateService.OnStateUpdated -= OnStateUpdated;
			_updateService.RemoveHandler<AttackCommand>(OnAttackUnit);
			_updateService.RemoveHandler<KillUnitCommand>(OnKillUnit);
		}

		void OnUnitSelected(ulong id) {
			UpdateSelection(id == _model.State.Id);
		}

		void OnUnitCanTurn(ulong? id, bool canTurn) {
			if ( !_model.IsPlayerUnit ) {
				return;
			}
			if ( id.HasValue && (id != _model.State.Id) ) {
				return;
			}
			UpdateInteractable(canTurn);
		}

		void UpdateHealth() {
			var maxHealth = _model.Config.MaxHealth[_model.State.Level];
			var normalized = (float)_model.State.Health / maxHealth;
			_view.UpdateHealth(normalized);
		}

		void UpdateSelection(bool active) {
			Selection.SetActive(active);
		}

		void UpdateInteractable(bool interactable) {
			Interactable.SetActive(interactable);
		}

		async Task OnAttackUnit(AttackCommand cmd) {
			if ( cmd.TargetId == _model.State.Id ) {
				var diff = _oldHealth - _model.State.Health;
				_oldHealth = _model.State.Health;
				await _view.AnimateDamage(diff);
			}
		}
		
		async Task OnKillUnit(KillUnitCommand cmd) {
			if ( cmd.UnitId == _model.State.Id ) {
				await transform.DoScale(0.5f, 0.0f);
				gameObject.SetActive(false);
				_view.gameObject.SetActive(false);
			}
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

		void SelectView() {
			var wantedName = _model.State.Descriptor;
			for ( var i = 0; i < ViewRoot.childCount; i++ ) {
				var child = ViewRoot.GetChild(i).gameObject;
				child.SetActive(child.name == wantedName);
			}
		}
	}
}
