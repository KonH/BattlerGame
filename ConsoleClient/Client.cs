using GameLogics.Client.Service;
using GameLogics.Server.Repository.Config;
using GameLogics.Server.Repository.State;
using GameLogics.Server.Repository.User;
using GameLogics.Server.Service;
using GameLogics.Server.Service.Token;
using GameLogics.Shared.Service;
using GameLogics.Shared.Service.Time;
using AuthService = GameLogics.Client.Service.AuthService;
using RegisterService = GameLogics.Client.Service.RegisterService;

namespace ConsoleClient {
	public sealed class Client {
		public ICustomLogger          Logger     { get; } = new ConsoleLogger();
		public ConvertService         Convert    { get; } = new ConvertService();
		public ClientStateService     State      { get; } = new ClientStateService();
		public ITimeService           Time       { get; } = new RealTimeService();
		public INetworkService        Network    { get; }
		public OffsetTimeService      OffsetTime { get; private set; }
		public IApiService            Api        { get; private set; }
		public RegisterService        Register   { get; private set; }
		public AuthService            Auth       { get; private set; }
		public GameStateUpdateService Updater    { get; private set; }

		public Client() {
			Network = new HttpClientNetworkService(Logger, "http://localhost:8080/");
		}
		
		public Client AddClientApiService() {
			Api = new ClientApiService(Logger, Convert, Network, new TerminateErrorHandleStrategy(Logger));
			
			AddCommon();
			
			return this;
		}

		public Client AddServerApiService() {
			var env      = new EnvironmentService { IsDebugMode = true };
			var users    = new InMemoryUserRepository();
			var states   = new InMemoryGameStateRepository();
			var register = new GameLogics.Server.Service.RegisterService(users);
			var config   = new FileConfigRepository(Convert, "Config.json");
			var auth     = new GameLogics.Server.Service.AuthService(Logger, new MockTokenService(), Time, users, states, config, new StateInitService());
			var intent   = new IntentService(env, Logger, Time, users, states, config);

			Api = new ConvertedServerApiService(Convert, Logger, new TerminateErrorHandleStrategy(Logger), register, auth, intent);
			
			AddCommon();
			
			return this;
		}

		void AddCommon() {
			OffsetTime = new OffsetTimeService(Time);
			Register = new RegisterService(Api, State);
			Auth     = new AuthService(Api, Network, OffsetTime, State);
			Updater = new GameStateUpdateService(Logger, OffsetTime, Api, State);
		}
	}
}