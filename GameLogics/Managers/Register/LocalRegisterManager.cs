using System.Threading.Tasks;

namespace GameLogics.Managers.Register {
	public class LocalRegisterManager : IRegisterManager {
		public Task<bool> TryRegister() {
			return Task.FromResult<bool>(true);
		}
	}
}