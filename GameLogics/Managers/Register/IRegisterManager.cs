using System.Threading.Tasks;

namespace GameLogics.Managers.Register {
	public interface IRegisterManager {
		Task<bool> TryRegister();
	}
}