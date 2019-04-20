using System.IO;
using GameLogics.Server.Repository;
using GameLogics.Server.Repository.Config;
using GameLogics.Server.Repository.State;
using GameLogics.Server.Repository.User;
using GameLogics.Server.Service;
using GameLogics.Server.Service.Token;
using GameLogics.Shared.Service;
using UnityClient.Service;
using UnityEngine;
using Zenject;

namespace UnityClient.Installer {
	public static class ServerExtensions {
		public static void BindEmbeddedService(this DiContainer self, bool inMemory) {
			self.Bind<IApiService>().To<ConvertedServerApiService>().AsSingle();

			self.Bind<ITokenService>().To<MockTokenService>().AsSingle();

			if ( inMemory ) {
				self.Bind<IUserRepository>().To<InMemoryUserRepository>().AsSingle();
				self.Bind<IGameStateRepository>().To<InMemoryGameStateRepository>().AsSingle();
			} else {
				self.Bind<FileStorageRepository>().FromMethod(CreateFileStorage).AsSingle();
				self.Bind<IUserRepository>().To<FileUserRepository>().AsSingle();
				self.Bind<IGameStateRepository>().To<FileGameStateRepository>().AsSingle();
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