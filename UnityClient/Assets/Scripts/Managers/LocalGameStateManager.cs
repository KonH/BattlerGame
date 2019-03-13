using GameLogics.Managers;
using GameLogics.Models;
using GameLogics.Repositories.State;
using Zenject;

namespace UnityClient.Managers {
	public class LocalGameStateManager : IInitializable {
		readonly LocalGameStateRepository _stateRepository;
		
		public LocalGameStateManager(LocalGameStateRepository stateRepository, CommandExecutor executor) {
			_stateRepository = stateRepository;
			executor.OnStateUpdated += OnStateUpdated;
		}
		
		public void Initialize() {
			_stateRepository.Load();
		}

		void OnStateUpdated(GameState state) {
			_stateRepository.Save();
		}
	}
}