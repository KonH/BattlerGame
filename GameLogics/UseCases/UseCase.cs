using System.Collections.Generic;
using GameLogics.Commands;
using GameLogics.Intents;

namespace GameLogics.UseCases {
	public abstract class UseCase<TIntent> : IUseCase where TIntent : class, IIntent {
		public abstract void Execute(TIntent intent, List<ICommand> result);
		
		public List<ICommand> Execute(IIntent intent) {
			var result = new List<ICommand>();
			Execute(intent as TIntent, result);
			return result;
		}
	}
}