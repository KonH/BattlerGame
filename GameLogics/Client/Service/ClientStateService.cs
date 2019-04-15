using GameLogics.Client.Model;
using GameLogics.Shared.Model.Config;
using GameLogics.Shared.Model.State;

namespace GameLogics.Client.Service {
	public sealed class ClientStateService {
		public GameState  State  { get; set; }
		public ConfigRoot Config { get; set; }
		public UserState  User   { get; set; }
	}
}