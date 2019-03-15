using System.Threading.Tasks;
using GameLogics.Client.Utils;

namespace GameLogics.Client.Services {
	public interface INetworkService {
		string AuthToken { get; set; }
		Task<NetworkResponse> PostJson(string relativeUrl, string body);
	}
}