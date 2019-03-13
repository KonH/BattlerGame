using System.Threading.Tasks;

namespace GameLogics.Managers.Auth {
	public class LocalAuthManager : IAuthManager {
		public Task<bool> TryLogin() {
			return Task.FromResult(true);
		}
	}
}