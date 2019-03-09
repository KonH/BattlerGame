using System.Collections.Generic;
using GameLogics.Commands;
using GameLogics.Intents;

namespace GameLogics.UseCases {
	public interface IUseCase {
		List<ICommand> Execute(IIntent intent);
	}
}