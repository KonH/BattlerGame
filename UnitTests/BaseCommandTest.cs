using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using Xunit;

namespace UnitTests {
	public class BaseCommandTest<TCommand> where TCommand : class, ICommand {
		protected GameState _state  = new GameState();
		protected Config    _config = new Config();

		protected void IsValid(TCommand cmd) {
			Assert.True(cmd.IsValid(_state, _config));
		}
		
		protected void IsInvalid(TCommand cmd) {
			Assert.False(cmd.IsValid(_state, _config));
		}

		protected void Execute(TCommand cmd) {
			IsValid(cmd);
			cmd.Execute(_state, _config);
		}
	}
}