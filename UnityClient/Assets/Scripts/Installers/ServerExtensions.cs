using GameLogics.Server.Repositories.States;
using GameLogics.Server.Repositories.Users;
using GameLogics.Server.Services;
using GameLogics.Server.Services.Token;
using GameLogics.Shared.Services;
using Zenject;

namespace UnityClient.Installers {
	public static class ServerExtensions {
		public static void BindEmbeddedService(this DiContainer self) {
			self.Bind<IApiService>().To<ConvertedServerApiService>().AsSingle();

			self.Bind<ITokenService>().To<MockTokenService>().AsSingle();
			self.Bind<IntentToCommandMapper>().AsSingle();
			
			self.Bind<IUsersRepository>().To<InMemoryUsersRepository>().AsSingle();
			self.Bind<IGameStatesRepository>().To<InMemoryGameStatesRepository>().AsSingle();
			
			self.Bind<RegisterService>().AsSingle();
			self.Bind<AuthService>().AsSingle();
			self.Bind<IntentService>().AsSingle();
		}
	}
}