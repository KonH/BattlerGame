using GameLogics.Core;
using GameLogics.Managers;
using GameLogics.Utils;
using UnityEngine;
using Zenject;
using TMPro;

namespace UnityClient.Views {
	[RequireComponent(typeof(TMP_Text))]
	public class ResourceView : MonoBehaviour {
		public Resource Kind;

		TMP_Text _text;
		
		CommandExecutor _executor;

		[Inject]
		public void Init(IGameStateManager stateManager, CommandExecutor executor) {
			_text = GetComponent<TMP_Text>();
			_executor = executor;
			_executor.OnStateUpdated += UpdateState;
			UpdateState(stateManager.State);
		}

		void OnEnable() {
			if ( _executor != null ) {
				_executor.OnStateUpdated += UpdateState;
			}
		}

		void OnDisable() {
			if ( _executor != null ) {
				_executor.OnStateUpdated -= UpdateState;
			}
		}

		void UpdateState(GameState state) {
			var value = state.Resources.GetOrDefault(Kind);
			_text.text = value.ToString();
		}
	}
}
