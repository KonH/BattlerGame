using GameLogics.Commands;
using GameLogics.Core;
using UnityEngine;
using Zenject;

namespace UnityClient.Controls {
	public class ResourceControl : MonoBehaviour {
		public Resource Kind;
		public int      Amount;

		CommandExecutor _executor;
		
		[Inject]
		public void Init(CommandExecutor executor) {
			_executor = executor;
		}

		public void Execute() {
			_executor.Execute(new AddResourceCommand(Kind, Amount));
		}
	}
}

