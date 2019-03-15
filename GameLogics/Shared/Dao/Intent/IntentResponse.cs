using System.Collections.Generic;
using GameLogics.Shared.Commands;

namespace GameLogics.Shared.Dao.Intent {
	public class IntentResponse {
		public string         NewVersion { get; set; }
		public List<ICommand> Commands   { get; set; }

		public IntentResponse(string newVersion, List<ICommand> commands) {
			NewVersion = newVersion;
			Commands   = commands;
		}

		public override string ToString() {
			return $"NewVersion: '{NewVersion}', Commands: {string.Join(";", Commands)}";
		}
	}
}