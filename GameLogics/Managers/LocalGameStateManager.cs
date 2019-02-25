using System.IO;
using GameLogics.Core;
using Newtonsoft.Json;

namespace GameLogics.Managers {
	public sealed class LocalGameStateManager : IGameStateManager {
		public GameState State { get; private set; }

		readonly string _path;
		
		public LocalGameStateManager(string path) {
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