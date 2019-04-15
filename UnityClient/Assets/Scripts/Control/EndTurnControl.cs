using GameLogics.Shared.Command;
using GameLogics.Shared.Model.State;
using UnityClient.Service;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.Control {
	[RequireComponent(typeof(Button))]
	public sealed class EndTurnControl : MonoBehaviour {
		ClientCommandRunner _runner;
		
		Button _button;
		
		EndPlayerTurnCommand _command = new EndPlayerTurnCommand();
		
		[Inject]
		void Init(ClientCommandRunner runner) {
			_runner = runner;
			_button = GetComponent<Button>();
			_button.onClick.AddListener(Execute);
			_runner.Updater.OnStateUpdated += UpdateValidation;
			var state = _runner.Updater.State;
			if ( state != null ) {
				UpdateValidation(state);
			}
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
