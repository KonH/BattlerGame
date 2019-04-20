using GameLogics.Client.Service;
using GameLogics.Shared.Model;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Utils;
using UnityEngine;
using Zenject;
using TMPro;

namespace UnityClient.View {
	[RequireComponent(typeof(TMP_Text))]
	public sealed class ResourceView : MonoBehaviour {
		public Resource Kind;

		TMP_Text _text;
		
		GameStateUpdateService _service;

		[Inject]
		public void Init(GameStateUpdateService service) {
			_text = GetComponent<TMP_Text>();
			_service = service;
			_service.OnStateUpdated += UpdateState;
			if ( _service.State != null ) {
				UpdateState(service.State);
			}
		}

		void OnEnable() {
			if ( _service != null ) {
				_service.OnStateUpdated += UpdateState;
			}
		}

		void OnDisable() {
			if ( _service != null ) {
				_service.OnStateUpdated -= UpdateState;
			}
		}

		void UpdateState(GameState state) {
			var value = state.Resources.GetOrDefault(Kind);
			_text.text = value.ToString();
		}
	}
}
