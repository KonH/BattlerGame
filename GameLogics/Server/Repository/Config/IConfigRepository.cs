using GameLogics.Shared.Model.Config;

namespace GameLogics.Server.Repository.Config {
	public interface IConfigRepository {
		ConfigRoot Get();
	}
}