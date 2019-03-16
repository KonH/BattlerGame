using GameLogics.Shared.Models.Configs;

namespace GameLogics.Server.Repositories.Configs {
	public class InMemoryConfigRepository : IConfigRepository {
		readonly Config _instance = new Config {
			Version = "in_memory_config",
		};
		
		public Config Get() {
			return _instance;
		}
	}
}