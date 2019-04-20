using System.Collections.Generic;
using System.IO;
using GameLogics.Server.Model;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Service;

namespace GameLogics.Server.Repository {
	public sealed class FileStorageRepository {
		public class FileState {
			public Dictionary<string, UserState> Users  = new Dictionary<string, UserState>();
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