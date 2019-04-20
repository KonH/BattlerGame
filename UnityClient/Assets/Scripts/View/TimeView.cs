using GameLogics.Client.Service;
using GameLogics.Shared.Model;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Utils;
using GameLogics.Shared.Service.Time;
using UnityEngine;
using Zenject;
using TMPro;

namespace UnityClient.View {
	[RequireComponent(typeof(TMP_Text))]
	public sealed class TimeView : MonoBehaviour {
		public string Format;

		TMP_Text _text;

		ClientStateService _stateService;
		OffsetTimeService  _offsetTime;

		int _lastSecond = -1;

		[Inject]
		public void Init(ClientStateService stateService, OffsetTimeService offsetTime) {
			_text         = GetComponent<TMP_Text>();
			_stateService = stateService;
			_offsetTime   = offsetTime;
		}

		public void Update() {
			var time = _stateService.State.Time;
			var timeWithOffset = time.LastSyncTime + time.PersistentOffset + _offsetTime.Offset;
			var newSecond = timeWithOffset.Second;
			if ( newSecond == _lastSecond ) {
				return;
			}
			_lastSecond = newSecond;
			_text.text = timeWithOffset.ToString(Format);
		}
	}
}
