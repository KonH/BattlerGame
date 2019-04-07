using System.Threading.Tasks;
using GameLogics.Client.Services;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Models.State;
using TMPro;
using UnityClient.Models;
using UnityClient.Services;
using UnityClient.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.ViewModels {
	public sealed class UnitViewModel : MonoBehaviour, IPointerClickHandler {
		public sealed class Factory : PlaceholderFactory<UnitLevelModel, UnitViewModel> {}

		public Color PlayerHealthColor = Color.green;
		public Color EnemyHealthColor  = Color.red;

		public Transform  ViewRoot     = null;
		public GameObject Selection    = null;
		public GameObject Interactable = null;
		public Slider     HealthSlider = null;
		public Image      HealthImage  = null;
		public TMP_Text   DamageText   = null;
		
		GameStateUpdateService _updateService;
		LevelService           _levelService;
		UnitLevelModel         _model;

		int _oldHealth;

		[Inject]
		public void Init(GameStateUpdateService updateService, LevelService levelService, UnitLevelModel model) {
			_updateService = updateService;
			_levelService  = levelService;
			_model         = model;
			_oldHealth     = _model.State.Health;

			_levelService.OnUnitSelected += OnUnitSelected;
			_levelService.OnUnitCanTurn  += OnUnitCanTurn;
			_updateService.OnStateUpdated += OnStateUpdated;
			_updateService.AddHandler<AttackCommand>(OnAttackUnit);
			_updateService.AddHandler<KillUnitCommand>(OnKillUnit);

			HealthImage.color = model.IsPlayerUnit ? PlayerHealthColor : EnemyHealthColor;
			DamageText.text = "";
			SelectView();
			UpdateSelection(false);
			UpdateInteractable(model.IsPlayerUnit);
			UpdateHealth();
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
			HealthSlider.value = (float)_model.State.Health / _model.Config.MaxHealth;
		}

		void UpdateSelection(bool active) {
			Selection.SetActive(active);
		}

		void UpdateInteractable(bool interactable) {
			Interactable.SetActive(interactable);
		}

		async Task OnAttackUnit(AttackCommand cmd) {
			if ( cmd.TargetId == _model.State.Id ) {
				var diff = _model.State.Health;
				DamageText.text = (diff > 0) ? $"-{diff}" : "";
				var textTrans = DamageText.transform;
				textTrans.localScale = Vector3.zero;
				textTrans.DoScale(0.25f, 1.0f).Detach();
				await transform.DoScale(0.25f, 0.75f);
				transform.DoScale(0.25f, 1.0f).Detach();
				await textTrans.DoScale(0.5f, 0.0f);
			}
		}
		
		async Task OnKillUnit(KillUnitCommand cmd) {
			if ( cmd.UnitId == _model.State.Id ) {
				await transform.DoScale(0.5f, 0.0f);
				gameObject.SetActive(false);
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
