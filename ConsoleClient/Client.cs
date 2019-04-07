using GameLogics.Client.Services;
using GameLogics.Server.Repositories.Configs;
using GameLogics.Server.Repositories.States;
using GameLogics.Server.Repositories.Users;
using GameLogics.Server.Services;
using GameLogics.Server.Services.Token;
using GameLogics.Shared.Services;
using AuthService = GameLogics.Client.Services.AuthService;
using RegisterService = GameLogics.Client.Services.RegisterService;

namespace ConsoleClient {
	public sealed class Client {
		public ICustomLogger          Logger   { get; } = new ConsoleLogger();
		public ConvertService         Convert  { get; } = new ConvertService();
		public ClientStateService     State    { get; } = new ClientStateService();
		public INetworkService        Network  { get; }
		public IApiService            Api      { get; private set; }
		public RegisterService        Register { get; private set; }
		public AuthService            Auth     { get; private set; }
		public GameStateUpdateService Updater  { get; private set; }

		public Client() {
			Network = new HttpClientNetworkService(Logger, "http://localhost:8080/");
		}
		
		public Client AddClientApiService() {
			Api = new ClientApiService(Logger, Convert, Network, new TerminateErrorHandleStrategy(Logger));
			
			AddCommon();
			
			return this;
		}

		public Client AddServerApiService() {
			var users    = new InMemoryUsersRepository();
			var states   = new InMemoryGameStatesRepository();
			var register = new GameLogics.Server.Services.RegisterService(users);
			var config   = new FileConfigRepository(Convert, "../UnityClient/Assets/Resources/Config.json");
			var auth     = new GameLogics.Server.Services.AuthService(Logger, new MockTokenService(), users, states, config, new StateInitService());
			var intent   = new IntentService(Logger, users, states, config);
			
			Api = new ConvertedServerApiService(Convert, Logger, new TerminateErrorHandleStrategy(Logger), register, auth, intent);
			
			AddCommon();
			
			return this;
		}

		void AddCommon() {
			Register = new RegisterService(Api, State);
			Auth     = new AuthService(Api, Network, State);
			Updater = new GameStateUpdateService(Logger, Api, State);
		}
	}
}