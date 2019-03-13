using System.Collections.Generic;
using GameLogics.Commands;
using GameLogics.Intents;
using GameLogics.Models;

namespace GameLogics.UseCases {
	public abstract class UseCase<TIntent> : IUseCase where TIntent : class, IIntent {
		public abstract void Execute(GameState state, TIntent intent, List<ICommand> result);
		
		public List<ICommand> Execute(GameState state, IIntent intent) {
			var result = new List<ICommand>();
			Execute(state, intent as TIntent, result);
			return result;
		}
	}
}