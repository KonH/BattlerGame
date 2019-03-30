using System;
using System.Collections.Generic;
using GameLogics.Client.Services;
using UnityClient.Managers;
using UnityClient.Models;
using UnityClient.Services;
using UnityClient.ViewModels.Fragments;
using UnityClient.ViewModels.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.Controls {
	public class StartLevelControl : MonoBehaviour {
		public string LevelDesc;

		public Button Button;

		UiManager           _ui;
		ClientStateService  _service;
		ClientCommandRunner _runner;
		GameSceneManager    _scene;
		
		[Inject]
		public void Init(UiManager ui, ClientStateService service, ClientCommandRunner runner, GameSceneManager scene) {
			_ui      = ui;
			_service = service;
			_runner  = runner;
			_scene   = scene;
			
			Button.onClick.AddListener(Execute);
		}

		public void Execute() {
			_ui.ShowWindow<StartLevelWindow>(w => {
				Action<List<UnitModel>, string, Action<UnitModel>> act = (units, unitActName, unitAct) => {
					_ui.ShowWindow<UnitsWindow>(w2 => {
						Action<UnitModel> act2 = u => {
							w2.Animation.Hide(() => Destroy(w2.gameObject));
							unitAct(u);
						};
						w2.Show(units, _ui.GetFragmentTemplate<UnitFragment>(), unitActName, act2);
					});
				};
				w.Show(LevelDesc, _service, _runner, _ui.GetFragmentTemplate<UnitFragment>(), () => _scene.GoToLevel(), act);
			});
		}
	}
}

