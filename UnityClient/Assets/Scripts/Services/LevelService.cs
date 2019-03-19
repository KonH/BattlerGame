using System.Threading.Tasks;
using GameLogics.Client.Services;
using GameLogics.Shared.Commands;

namespace UnityClient.Services {
	public class LevelService {
		readonly GameStateUpdateService _updateService;

		ulong? _selectedUnitId;
		
		public LevelService(GameStateUpdateService updateService) {
			_updateService = updateService;
		}
		
		public void SelectUnit(ulong unitId) {
			_selectedUnitId = unitId;
		}

		public Task AttackUnit(ulong unitId) {
			if ( !_selectedUnitId.HasValue ) {
				return Task.CompletedTask;
			}
			return _updateService.Update(new AttackCommand(_selectedUnitId.Value, unitId));
		}
	}
}