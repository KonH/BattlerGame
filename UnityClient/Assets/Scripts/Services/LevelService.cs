using System.Threading.Tasks;
using GameLogics.Client.Services;
using GameLogics.Shared.Commands;

namespace UnityClient.Services {
	public class LevelService {
		readonly GameStateUpdateService _updateService;

		string _selectedUnitId = string.Empty;
		
		public LevelService(GameStateUpdateService updateService) {
			_updateService = updateService;
		}
		
		public void SelectUnit(string unitId) {
			_selectedUnitId = unitId;
		}

		public Task AttackUnit(string unitId) {
			if ( string.IsNullOrEmpty(_selectedUnitId) ) {
				return Task.CompletedTask;
			}
			return _updateService.Update(new AttackCommand(_selectedUnitId, unitId));
		}
	}
}