using System.Threading.Tasks;

namespace GameLogics.Managers.Auth {
	public interface IAuthManager {
		Task<bool> TryLogin();
	}
}