using System.Collections.Generic;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands {
	public abstract class BaseCommand : ICommand {
		protected readonly List<ICommand> NoSubCommands = new List<ICommand>();
		
		public abstract bool IsValid(GameState state, Config config);

		public virtual List<ICommand> Execute(GameState state, Config config) {
			ExecuteSingle(state, config);
			return NoSubCommands;
		}
		
		protected virtual void ExecuteSingle(GameState state, Config config) {}

		public bool TryExecute(GameState state, Config config) {
			if ( IsValid(state, config) ) {
				var subCommands = Execute(state, config);
				foreach ( var subCommand in subCommands ) {
					if ( !subCommand.TryExecute(state, config) ) {
						return false;
					}
				}
				return true;
			}
			return false;
		}
		
		public List<ICommand> GetAllSubCommands(GameState state, Config config) {
			var result = new List<ICommand>();
			var subCommands = Execute(state, config);
			foreach ( var subCommand in subCommands ) {
				result.Add(subCommand);
				result.AddRange(subCommand.GetAllSubCommands(state, config));
			}
			return result;
		}

		protected List<ICommand> WithSubCommand(ICommand cmd) {
			return new List<ICommand> { cmd };
		}
	}
}