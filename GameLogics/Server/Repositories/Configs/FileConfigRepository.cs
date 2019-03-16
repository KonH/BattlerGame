using GameLogics.Shared.Models.Configs;

namespace GameLogics.Server.Repositories.Configs {
	public class FileConfigRepository : IConfigRepository {
		readonly FileStorageRepository _storage;

		public FileConfigRepository(FileStorageRepository storage) {
			_storage = storage;
		}
		
		public Config Get() {
			return _storage.State.Config;
		}
	}
}