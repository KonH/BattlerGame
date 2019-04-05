using GameLogics.Client.Models;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Models.State;

namespace GameLogics.Client.Services {
	public class ClientStateService {
		public GameState State  { get; set; }
		public Config    Config { get; set; }
		public User      User   { get; set; }
	}
}