using System;
using System.Threading.Tasks;
using GameLogics.Client.Models;
using GameLogics.Client.Repositories;
using GameLogics.Client.Services;
using GameLogics.Server.Repositories.States;
using GameLogics.Server.Repositories.Users;
using GameLogics.Server.Services;
using GameLogics.Server.Services.Token;
using GameLogics.Shared.Intents;
using GameLogics.Shared.Models;
using GameLogics.Shared.Services;
using AuthService = GameLogics.Client.Services.AuthService;
using RegisterService = GameLogics.Client.Services.RegisterService;

namespace ConsoleClient {
	class Program {
		static ICustomLogger       _logger    = new ConsoleLogger();
		static INetworkService     _network   = new HttpClientNetworkService(_logger, "http://localhost:8080/");
		static ConvertService      _converter = new ConvertService();
		static IApiService         _api       = CreateClientApiService();
		static GameStateRepository _state     = new GameStateRepository();

		static UserRepository _user = new UserRepository {
			CurrentUser = new User("newUser", "password", "newUserName")
		};
		
		static void Main(string[] args) {
			Main().GetAwaiter().GetResult();
		}

		static IApiService CreateClientApiService() {
			return new ClientApiService(_logger, _converter, _network);
		}

		static IApiService CreateServerApiService() {
			var users = new InMemoryUsersRepository();
			var states = new InMemoryGameStatesRepository();
			var register = new GameLogics.Server.Services.RegisterService(users);
			var auth = new GameLogics.Server.Services.AuthService(_logger, users, states, new MockTokenService());
			var intent = new IntentService(_logger, users, states, new IntentToCommandMapper());
			return new ConvertedServerApiService(_converter, _logger, register, auth, intent);
		}

		static async Task Main() {
			await RegisterCase();
			await LoginCase();
			await AddResourceCase();
		}
		
		static async Task RegisterCase() {
			var register = new RegisterService(_api, _user);
			var result = await register.TryRegister();
			Console.WriteLine("RegisterCase: {0}", result);
		}
		
		static async Task LoginCase() {
			var auth = new AuthService(_api, _network, _user, _state);
			var result = await auth.TryLogin();
			Console.WriteLine("LoginCase: {0} ({1})", result, _user.CurrentUser.Token);
		}
		
		static async Task AddResourceCase() {
			Console.WriteLine($"AddResourceCase: starting Coins: {GetCoinsCount()}");
			var updater = new GameStateUpdateService(_api, _user, _state);
			updater.OnStateUpdated += _ => Console.WriteLine("AddResourceCase: state updated"); 
			await updater.Update(new RequestResourceIntent(Resource.Coins, 1));
			Console.WriteLine("AddResourceCase: result commands:");
			Console.WriteLine($"AddResourceCase: result Coins: {GetCoinsCount()}");
		}

		static string GetCoinsCount() {
			var state = _state.State;
			if ( state == null ) {
				return "no state";
			}
			return state.Resources.TryGetValue(Resource.Coins, out var coins) ? coins.ToString() : "none";
		}
	}
}