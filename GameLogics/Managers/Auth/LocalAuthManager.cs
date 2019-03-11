using System.Threading.Tasks;
using GameLogics.Models;

namespace GameLogics.Managers.Auth {
	public class LocalAuthManager : IAuthManager {		
		public Task<bool> TryLogin(User user) {
			return Task.FromResult(true);
		}
	}
}