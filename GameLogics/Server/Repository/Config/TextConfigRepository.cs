using GameLogics.Shared.Model.Config;
using GameLogics.Shared.Service;

namespace GameLogics.Server.Repository.Config {
	public sealed class TextConfigRepository : IConfigRepository {
		readonly ConfigRoot _config;

		public TextConfigRepository(ConvertService convert, string text) {
			_config = convert.FromJson<ConfigRoot>(text);
		}
		
		public ConfigRoot Get() {
			return _config;
		}
	}
}