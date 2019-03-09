using System.Collections.Generic;
using System.Linq;
using GameLogics.Commands;

namespace GameLogics.DAO {
	public class IntentResponse {
		public List<ICommand> Commands { get; set; }
		
		public override string ToString() {
			return $"Commands: {string.Join(";", Commands.Select(c => c.ToString()))}";
		}
	}
}