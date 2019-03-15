using System.Collections.Generic;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Intents;
using GameLogics.Shared.Models;

namespace GameLogics.Shared.UseCases {
	public class AddResourcesUseCase : UseCase<RequestResourceIntent> {
		public override void Execute(GameState state, RequestResourceIntent intent, List<ICommand> result) {
			result.Add(new AddResourceCommand(intent.Kind, intent.Count));
		}
	}
}