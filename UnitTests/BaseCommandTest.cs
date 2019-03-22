using System;
using System.Collections.Generic;
using GameLogics.Server.Services;
using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using Xunit;

namespace UnitTests {
	public class BaseCommandTest<TCommand> where TCommand : class, ICommand {
		protected GameState _state  = new GameState();
		protected Config    _config = new Config();

		protected ulong InvalidId => ulong.MaxValue;
		
		protected ulong NewId() {
			return _state.NewEntityId();
		}
		
		protected void IsValid(TCommand cmd) {
			Assert.True(cmd.IsValid(_state, _config));
		}
		
		protected void IsInvalid(TCommand cmd) {
			Assert.False(cmd.IsValid(_state, _config));
		}

		protected List<ICommand> Execute(TCommand cmd) {
			var result = new List<ICommand>();
			var runner = new CommandRunner(cmd, _state, _config);
			foreach ( var item in runner ) {
				Assert.True(item.IsValid(), $"Command {item.Command} is invalid!");
				item.Execute();
				result.Add(item.Command);
			}
			return result;
		}
		
		protected void Produces<TOtherCommand>(TCommand cmd, Func<TOtherCommand, bool> predicate = null) where TOtherCommand : ICommand {
			var commands = Execute(cmd);
			Assert.Contains(
				commands,
				c => (c is TOtherCommand oc) && ( (predicate == null) || predicate(oc) )
			);
		}
		
		protected void ProducesAll(TCommand cmd, Func<ICommand, bool> predicate) {
			var commands = Execute(cmd);
			Assert.Contains(
				commands,
				c => predicate(c)
			);
		}
		
		protected void ProducesNone(TCommand cmd) {
			Assert.Single(Execute(cmd));
		}

		protected void IsValidOnServer(TCommand cmd) {
			Assert.True(IntentService.IsValidAsFirstCommand(cmd));
		}
		
		protected void IsInvalidOnServer(TCommand cmd) {
			Assert.False(IntentService.IsValidAsFirstCommand(cmd));
		}
	}
}