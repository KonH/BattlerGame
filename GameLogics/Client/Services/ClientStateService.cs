using GameLogics.Client.Models;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Client.Services {
	public class ClientStateService {
		public GameState State  { get; set; }
		public Config    Config { get; set; }
		public User      User   { get; set; }
	}
}