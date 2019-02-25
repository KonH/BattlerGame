using GameLogics.Commands;
using GameLogics.Core;
using GameLogics.Utils;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.Views {
	[RequireComponent(typeof(Text))]
	public class ResourceView : MonoBehaviour {
		public Resource Kind;

		Text _text;
		
		CommandExecutor _executor;

		[Inject]
		public void Init(GameState state, CommandExecutor executor) {
			_text = GetComponent<Text>();
			_executor = executor;
			_executor.OnStateUpdated += UpdateState;
			UpdateState(state);
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
