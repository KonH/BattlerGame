using System;
using GameLogics.Shared.Command;
using GameLogics.Shared.Command.Base;
using GameLogics.Shared.Model;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;

namespace GameLogics.Server.Service {
	public sealed class StateInitService {
		Random _realRandom = new Random();
		
		class InitCommand : IInternalCommand {
			public bool IsValid(GameState state, ConfigRoot config) => true;

			public void Execute(GameState state, ConfigRoot config, ICommandBuffer buffer) {
				buffer.Add(new AddResourceCommand(Resource.Coins, 50));
				buffer.Add(new AddUnitCommand(state.NewEntityId(), "weak_unit"));
			}
		}
		
		public GameState Init(GameState state, ConfigRoot config) {
			state.Random.Seed = _realRandom.Next(int.MinValue, int.MaxValue);
			var runner = new CommandRunner(TimeSpan.Zero, new InitCommand(), state, config);
			foreach ( var item in runner ) {
				if ( !item.IsValid() ) {
					throw new InvalidOperationException($"Can't init state with {item.Command}");
				}
				item.Execute();
			}
			return state;
		}
	}
}