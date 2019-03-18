using System.IO;
using GameLogics.Server.Repositories;
using GameLogics.Server.Repositories.Configs;
using GameLogics.Server.Repositories.States;
using GameLogics.Server.Repositories.Users;
using GameLogics.Server.Services;
using GameLogics.Server.Services.Token;
using GameLogics.Shared.Services;
using UnityClient.Services;
using UnityEngine;
using Zenject;

namespace UnityClient.Installers {
	public static class ServerExtensions {
		public static void BindEmbeddedService(this DiContainer self, bool inMemory) {
			self.Bind<IApiService>().To<ConvertedServerApiService>().AsSingle();

			self.Bind<ITokenService>().To<MockTokenService>().AsSingle();

			if ( inMemory ) {
				self.Bind<IUsersRepository>().To<InMemoryUsersRepository>().AsSingle();
				self.Bind<IGameStatesRepository>().To<InMemoryGameStatesRepository>().AsSingle();
			} else {
				self.Bind<FileStorageRepository>().FromMethod(CreateFileStorage).AsSingle();
				self.Bind<IUsersRepository>().To<FileUsersRepository>().AsSingle();
				self.Bind<IGameStatesRepository>().To<FileGameStatesRepository>().AsSingle();
			}

			self.Bind<IConfigRepository>().FromMethod(CreateConfigRepository).AsSingle();

			self.Bind<RegisterService>().AsSingle();
			self.Bind<AuthService>().AsSingle();
			self.Bind<IntentService>().AsSingle();

			self.Bind<StateInitService>().AsSingle();
		}

		static FileStorageRepository CreateFileStorage(InjectContext context) {
			var convert  = context.Container.Resolve<ConvertService>();
			var settings = context.Container.Resolve<ServerSettings>();
			var path     = Path.Combine(Application.persistentDataPath, settings.EmbeddedDbName);
			return new FileStorageRepository(convert, path);
		}

		static IConfigRepository CreateConfigRepository(InjectContext context) {
			var convert = context.Container.Resolve<ConvertService>();
			var content = (Resources.Load("Config") as TextAsset).text;
			return new TextConfigRepository(convert, content);
		}
	}
}