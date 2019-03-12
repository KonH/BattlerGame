using System.Threading.Tasks;
using GameLogics.Models;

namespace GameLogics.Managers.Auth {
	public interface IAuthManager {
		Task<bool> TryLogin();
	}
}