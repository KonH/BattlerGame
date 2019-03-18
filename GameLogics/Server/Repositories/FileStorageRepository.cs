using System.Collections.Generic;
using System.IO;
using GameLogics.Server.Models;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Services;

namespace GameLogics.Server.Repositories {
	public class FileStorageRepository {
		public class FileState {
			public Dictionary<string, User>      Users  = new Dictionary<string, User>();
			public Dictionary<string, GameState> States = new Dictionary<string, GameState>();
		}
		
		readonly ConvertService _convert;
		readonly string         _path;

		public FileState State { get; } = new FileState();
		
		public FileStorageRepository(ConvertService convert, string path) {
			_convert = convert;
			_path    = path;
			if ( File.Exists(path) ) {
				var contents = File.ReadAllText(path);
				State = _convert.FromJson<FileState>(contents);
			}
		}

		public void Save() {
			var contents = _convert.ToJson(State);
			File.WriteAllText(_path, contents);
		}
	}
}