using System.IO;
using GameLogics.Shared.Model.Config;
using GameLogics.Shared.Service;

namespace GameLogics.Server.Repository.Config {
	public sealed class FileConfigRepository : IConfigRepository {
		readonly ConfigRoot _config;

		public FileConfigRepository(ConvertService convert, string path) {
			_config = convert.FromJson<ConfigRoot>(File.ReadAllText(path));
		}
		
		public ConfigRoot Get() {
			return _config;
		}
	}
}