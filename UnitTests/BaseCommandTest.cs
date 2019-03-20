using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using GameLogics.Server.Services;
using GameLogics.Shared.Commands;
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

		protected void Execute(TCommand cmd) {
			IsValid(cmd);
			var _ = cmd.Execute(_state, _config);
		}
		
		protected void Produces<TOtherCommand>(TCommand cmd, Func<TOtherCommand, bool> predicate = null) where TOtherCommand : ICommand {
			IsValid(cmd);
			Assert.Contains(
				cmd.Execute(_state, _config),
				c => (c is TOtherCommand oc) && ( (predicate == null) || predicate(oc) )
			);
		}
		
		protected void ProducesAll(TCommand cmd, Func<ICommand, bool> predicate) {
			IsValid(cmd);
			Assert.Contains(
				cmd.Execute(_state, _config),
				c => predicate(c)
			);
		}

		protected List<ICommand> GetAllSubCommands(TCommand cmd) {
			return cmd.GetAllSubCommands(_state, _config);
		}

		protected void ProducesInSubCommands<TOtherCommand>(TCommand cmd, Func<TOtherCommand, bool> predicate = null) where TOtherCommand : ICommand {
			IsValid(cmd);
			var produces = GetAllSubCommands(cmd);
			Assert.Contains(
				produces,
				c => (c is TOtherCommand oc) && ((predicate == null) || predicate(oc)));
		}
		
		protected void ProducesNone(TCommand cmd) {
			IsValid(cmd);
			Assert.Empty(cmd.Execute(_state, _config));
		}

		protected void IsValidOnServer(TCommand cmd) {
			Assert.True(IntentService.TryExecuteClientCommand(_state, _config, cmd));
		}
		
		protected void IsInvalidOnServer(TCommand cmd) {
			Assert.False(IntentService.TryExecuteClientCommand(_state, _config, cmd));
		}
	}
}