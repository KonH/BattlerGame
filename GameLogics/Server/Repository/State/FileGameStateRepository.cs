using System;
using GameLogics.Server.Model;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Utils;

namespace GameLogics.Server.Repository.State {
	public sealed class FileGameStateRepository : IGameStateRepository {
		FileStorageRepository _file;

		public FileGameStateRepository(FileStorageRepository file) {
			_file = file;
		}
		
		public GameState Find(UserState user) {
			return _file.State.States.GetOrDefault(user.Login);
		}

		public GameState FindOrCreate(UserState user, Action<GameState> init) {
			var state = Find(user);
			if ( state == null ) {
				state = new GameState();
				init(state);
			}
			return state;
		}

		public GameState Save(UserState user, GameState state) {
			_file.State.States[user.Login] = state;
			_file.Save();
			return state;
		}
	}
}