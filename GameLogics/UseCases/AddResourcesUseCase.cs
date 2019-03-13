using System.Collections.Generic;
using GameLogics.Commands;
using GameLogics.Intents;
using GameLogics.Models;

namespace GameLogics.UseCases {
	public class AddResourcesUseCase : UseCase<RequestResourceIntent> {
		public override void Execute(GameState state, RequestResourceIntent intent, List<ICommand> result) {
			result.Add(new AddResourceCommand(intent.Kind, intent.Count));
		}
	}
}