using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Services;

namespace GameLogics.Server.Repositories.Configs {
	public class TextConfigRepository : IConfigRepository {
		readonly Config _config;

		public TextConfigRepository(ConvertService convert, string text) {
			_config = convert.FromJson<Config>(text);
		}
		
		public Config Get() {
			return _config;
		}
	}
}