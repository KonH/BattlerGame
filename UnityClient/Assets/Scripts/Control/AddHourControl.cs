using GameLogics.Shared.Command;
using System;
using UnityClient.Service;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.Control {
	public sealed class AddHourControl : MonoBehaviour {
		public Button Button;

		ClientCommandRunner _runner;
		
		[Inject]
		public void Init(ClientCommandRunner runner) {
			_runner = runner;
			Button.onClick.AddListener(Execute);
		}

		public void Execute() {
			_runner.TryAddCommand(new AddPersistentTimeOffsetCommand(TimeSpan.FromHours(1)));
		}
	}
}

