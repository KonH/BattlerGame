﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameLogics.Client.Service;
using GameLogics.Shared.Command;
using GameLogics.Shared.Model.State;
using UnityClient.Model;
using UnityClient.Service;
using UnityClient.View;
using UnityClient.ViewModel;
using UnityClient.ViewModel.Window;
using UnityEngine;
using Zenject;

namespace UnityClient.Manager {
	public sealed class LevelManager : MonoBehaviour, IInitializable, IDisposable {
		public Transform[] PlayerPoints = null;
		public Transform[] EnemyPoints  = null;

		GameSceneManager       _scene;
		GameStateUpdateService _update;
		ClientStateService     _state;
		LevelService           _service;
		UnitViewModel.Factory  _unit;
		UnitView.Factory       _view;
		RewardWindow.Factory   _rewardWindow;
		LoseWindow.Factory     _loseWindow;
		
		[Inject]
		public void Init(
			GameSceneManager scene, ClientCommandRunner runner, ClientStateService state,
			LevelService service, UnitViewModel.Factory unit, UnitView.Factory view, RewardWindow.Factory winWindow, LoseWindow.Factory loseWindow
		) {
			_scene        = scene;
			_update       = runner.Updater;
			_state        = state;
			_service      = service;
			_unit         = unit;
			_view         = view;
			_rewardWindow = winWindow;
			_loseWindow   = loseWindow;
		}

		public void Initialize() {
			_update.AddHandler<EndPlayerTurnCommand>(OnEndPlayerTurn);
			_update.AddHandler<EndEnemyTurnCommand> (OnEndEnemyTurn);
			_update.AddHandler<FinishLevelCommand>  (OnFinishLevel);
			
			var level = _state.State?.Level;
			if ( level == null ) {
				return;
			}
			CreateUnits(level);
		}
		
		public void Dispose() {
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
				var ctx = new RewardWindow.Context("Level completed!", "Finish", _scene.GoToWorld);
				_rewardWindow.Create(ctx);
			} else {
				_loseWindow.Create(_scene.GoToWorld);
			}
			return Task.CompletedTask;
		}

		void CreateUnits(LevelState level) {
			CreateUnits(true, level.PlayerUnits, PlayerPoints);
			CreateUnits(false, level.EnemyUnits, EnemyPoints);
		}

		void CreateUnits(bool isPlayerUnit, List<UnitState> units, Transform[] points) {
			for ( var i = 0; i < units.Count; i++ ) {
				AddUnit(isPlayerUnit, units[i], points, i);
			}
		}
		
		void AddUnit(bool isPlayerUnit, UnitState state, Transform[] points, int position) {
			var config = _state.Config.Units[state.Descriptor];
			var model = new UnitLevelModel(isPlayerUnit, state, config);
			var view = _view.Create();
			_unit.Create(points[position], model, view);
		}
	}
}