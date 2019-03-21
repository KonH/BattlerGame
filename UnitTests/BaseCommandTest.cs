using System;
using System.Collections.Generic;
using GameLogics.Server.Services;
using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using Xunit;

namespace UnitTests {
	public class BaseCommandTest<TCommand> where TCommand : class, ICompositeCommand {
		protected GameState _state  = new GameState();
		protected Config    _config = new Config();

		protected ulong InvalidId => ulong.MaxValue;
		
		protected ulong NewId() {
			return _state.NewEntityId();
		}
		
		protected void IsValid(TCommand cmd) {
			Assert.True(cmd.IsFirstCommandValid(_state, _config));
		}
		
		protected void IsInvalid(TCommand cmd) {
			Assert.False(cmd.IsFirstCommandValid(_state, _config));
		}

		protected List<ICommand> Execute(TCommand cmd) {
			var result = new List<ICommand>();
			foreach ( var c in cmd.AsEnumerable() ) {
				Assert.True(c.IsCommandValid(_state, _config), $"Command {c} is invalid!");
				c.ExecuteCommand(_state, _config);
				result.Add(c);
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