using System.Collections.Generic;
using GameLogics.Commands;
using GameLogics.Intents;
using GameLogics.Models;

namespace GameLogics.UseCases {
	public interface IUseCase {
		List<ICommand> Execute(GameState state, IIntent intent);
	}
}