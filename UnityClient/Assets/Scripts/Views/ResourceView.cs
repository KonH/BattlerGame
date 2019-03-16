using GameLogics.Client.Services;
using GameLogics.Shared.Models;
using GameLogics.Shared.Utils;
using UnityEngine;
using Zenject;
using TMPro;

namespace UnityClient.Views {
	[RequireComponent(typeof(TMP_Text))]
	public class ResourceView : MonoBehaviour {
		public Resource Kind;

		TMP_Text _text;
		
		GameStateUpdateService _service;

		[Inject]
		public void Init(GameStateUpdateService service) {
			_text = GetComponent<TMP_Text>();
			_service = service;
			_service.OnStateUpdated += UpdateState;
			UpdateState(service.State);
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
