using System.Threading.Tasks;

namespace GameLogics.Managers.Network {
	public interface INetworkManager {
		string AuthToken { get; set; }
		Task<NetworkResponse> PostJson(string relativeUrl, string body);
	}
}