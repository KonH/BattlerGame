using Newtonsoft.Json;

namespace GameLogics.Shared.Service {
	public sealed class ConvertService {
		readonly JsonSerializerSettings _settings = new JsonSerializerSettings {
			TypeNameHandling = TypeNameHandling.Auto,
			Formatting       = Formatting.Indented,
		};

		public string ToJson(object obj) {
			return JsonConvert.SerializeObject(obj, _settings);
		}

		public T FromJson<T>(string json) {
			return JsonConvert.DeserializeObject<T>(json, _settings);
		}
		
		public T DoubleConvert<T>(T obj) {
			var json   = ToJson(obj);
			var newObj = FromJson<T>(json);
			return newObj;
		}
	}
}