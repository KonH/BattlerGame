using System.Threading.Tasks;
using GameLogics.Client.Utils;

namespace GameLogics.Client.Service {
	public interface INetworkService {
		string AuthToken { get; set; }
		Task<NetworkResponse> PostJson(string relativeUrl, string body);
	}
}