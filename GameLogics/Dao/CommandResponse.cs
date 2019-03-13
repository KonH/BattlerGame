using System.Collections.Generic;
using GameLogics.Commands;
using GameLogics.DAO.Errors;

namespace GameLogics.DAO {
	public class CommandResponse {
		public string         Version  { get; set; }
		public List<ICommand> Commands { get; set; }
		public ICommandError  Error    { get; set; }

		public bool Success => Error == null; 

		public CommandResponse(string version, List<ICommand> commands, ICommandError error) {
			Version  = version;
			Commands = commands;
			Error    = error;
		}

		public override string ToString() {
			return $"Version: '{Version}', Commands: {string.Join(";", Commands)}, Error: {Error}";
		}

		public static CommandResponse Failed(ICommandError error) => new CommandResponse(null, null, error);
		
		public static CommandResponse FromCommands(string version, List<ICommand> commands) => new CommandResponse(version, commands, null);
	}
}