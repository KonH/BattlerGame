using GameLogics.Shared.Models.Configs;

namespace GameLogics.Server.Repositories.Configs {
	public interface IConfigRepository {
		Config Get();
	}
}