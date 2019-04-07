using System.IO;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Services;

namespace GameLogics.Server.Repositories.Configs {
	public sealed class FileConfigRepository : IConfigRepository {
		readonly Config _config;

		public FileConfigRepository(ConvertService convert, string path) {
			_config = convert.FromJson<Config>(File.ReadAllText(path));
		}
		
		public Config Get() {
			return _config;
		}
	}
}