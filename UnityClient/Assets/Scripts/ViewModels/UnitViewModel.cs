﻿using System.Threading.Tasks;
using GameLogics.Client.Services;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models;
using UnityClient.Models;
using UnityClient.Services;
using UnityClient.Utils;
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
		public GameObject Interactable = null;
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
			_levelService.OnUnitCanTurn  += OnUnitCanTurn;
			_updateService.OnStateUpdated += OnStateUpdated;
			_updateService.AddHandler<AttackCommand>(OnAttackUnit);
			_updateService.AddHandler<KillUnitCommand>(OnKillUnit);

			HealthImage.color = model.IsPlayerUnit ? PlayerHealthColor : EnemyHealthColor;
			UpdateSelection(false);
			UpdateInteractable(true);
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

		async Task OnAttackUnit(ICommand c) {
			var cmd = (AttackCommand)c;
			if ( cmd.TargetId == _model.State.Id ) {
				await transform.DoScale(0.25f, 0.75f);
				await transform.DoScale(0.25f, 1.0f);
			}
		}
		
		async Task OnKillUnit(ICommand c) {
			var cmd = (KillUnitCommand)c;
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
	}
}
