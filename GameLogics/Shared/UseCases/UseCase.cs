using System.Collections.Generic;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Intents;
using GameLogics.Shared.Models;

namespace GameLogics.Shared.UseCases {
	public abstract class UseCase<TIntent> : IUseCase where TIntent : class, IIntent {
		public abstract void Execute(GameState state, TIntent intent, List<ICommand> result);
		
		public List<ICommand> Execute(GameState state, IIntent intent) {
			var result = new List<ICommand>();
			Execute(state, intent as TIntent, result);
			return result;
		}
	}
}