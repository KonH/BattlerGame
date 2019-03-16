using System;
using GameLogics.Server.Models;
using GameLogics.Shared.Models;
using GameLogics.Shared.Utils;

namespace GameLogics.Server.Repositories.States {
	public class FileGameStatesRepository : IGameStatesRepository {
		FileStorageRepository _file;

		public FileGameStatesRepository(FileStorageRepository file) {
			_file = file;
		}
		
		public GameState Find(User user) {
			return _file.State.States.GetOrDefault(user.Login);
		}

		public GameState FindOrCreate(User user, Action<GameState> init) {
			var state = Find(user);
			if ( state == null ) {
				state = new GameState();
				init(state);
			}
			return state;
		}

		public GameState Save(User user, GameState state) {
			_file.State.States[user.Login] = state;
			_file.Save();
			return state;
		}
	}
}