using System.Threading.Tasks;

namespace GameLogics.Managers.Network {
	public interface INetworkManager {
		Task<NetworkResponse> PostJson(string relativeUrl, string body);
	}
}