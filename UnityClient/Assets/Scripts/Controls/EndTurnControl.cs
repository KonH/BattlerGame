using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using UnityClient.Managers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.Controls {
	[RequireComponent(typeof(Button))]
	public class EndTurnControl : MonoBehaviour {
		CommandRunner _runner;
		
		Button _button;
		
		EndPlayerTurnCommand _command = new EndPlayerTurnCommand();
		
		[Inject]
		void Init(CommandRunner runner) {
			_runner = runner;
			_button = GetComponent<Button>();
			_button.onClick.AddListener(Execute);
			_runner.Updater.OnStateUpdated += UpdateValidation;
			UpdateValidation(_runner.Updater.State);
		}

		void OnDestroy() {
			if ( _runner != null ) {
				_runner.Updater.OnStateUpdated -= UpdateValidation;
			}
		}

		void Execute() {
			_runner.TryAddCommand(_command);
		}

		void UpdateValidation(GameState _) {
			_button.interactable = _runner.IsValid(_command);
		}
	}
}
