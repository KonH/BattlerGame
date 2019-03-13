using System.IO;
using GameLogics.Models;
using Newtonsoft.Json;

namespace GameLogics.Repositories.State {
	public sealed class LocalGameStateRepository : IGameStateRepository {
		public string    Version { get; set; }
		public GameState State   { get; set; }

		readonly string _path;
		
		public LocalGameStateRepository(string path) {
			_path = path;
		}
		
		public void Load() {
			if ( File.Exists(_path) ) {
				var content = File.ReadAllText(_path);
				State = JsonConvert.DeserializeObject<GameState>(content);
			}
			if ( State == null ) {
				State = new GameState();
			}
		}

		public void Save() {
			var content = JsonConvert.SerializeObject(State);
			File.WriteAllText(_path, content);
		}
	}
}