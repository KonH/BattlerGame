using System.Collections.Generic;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Intents;
using GameLogics.Shared.Models;

namespace GameLogics.Shared.UseCases {
	public interface IUseCase {
		List<ICommand> Execute(GameState state, IIntent intent);
	}
}