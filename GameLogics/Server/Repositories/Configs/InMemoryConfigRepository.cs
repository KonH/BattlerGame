using GameLogics.Shared.Models.Configs;

namespace GameLogics.Server.Repositories.Configs {
	public class InMemoryConfigRepository : IConfigRepository {
		readonly Config _instance = new Config {
			Version = "in_memory_config",
			Units = {
				{ "test_unit", new UnitConfig() },
				{ "test_enemy", new UnitConfig() },
			},
			Levels = {
				{ "test_level", new LevelConfig { EnemyDescriptors = { "test_enemy" } } }
			}
		};
		
		public Config Get() {
			return _instance;
		}
	}
}