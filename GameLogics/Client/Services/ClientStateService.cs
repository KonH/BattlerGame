using GameLogics.Client.Models;
using GameLogics.Shared.Models;

namespace GameLogics.Client.Services {
	public class ClientStateService {
		public GameState State { get; set; }
		public User      User  { get; set; }
	}
}