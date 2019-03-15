using Newtonsoft.Json;

namespace GameLogics.Shared.Services {
	public class ConvertService {
		readonly JsonSerializerSettings _settings = new JsonSerializerSettings {
			TypeNameHandling = TypeNameHandling.Auto
		};

		public string ToJson(object obj) {
			return JsonConvert.SerializeObject(obj, _settings);
		}

		public T FromJson<T>(string json) {
			return JsonConvert.DeserializeObject<T>(json, _settings);
		}
	}
}