using System;
using System.IO;
using GameLogics.Core;
using GameLogics.Managers;
using GameLogics.Repositories.State;
using Xunit;

namespace UnitTests {
	public class LocalGameStateManagerTest : IDisposable {
		const string _path = "file.json";
		
		LocalGameStateRepository _repository = new LocalGameStateRepository(_path);
		
		public void Dispose() {
			if ( File.Exists(_path) ) {
				File.Delete(_path);
			}
		}
		
		[Fact]
		public void FileIsSaved() {
			_repository.Load();
			_repository.Save();
			Assert.True(File.Exists(_path));
		}

		[Fact]
		public void ResourceIsSaved() {
			_repository.Load();
			_repository.State.Resources.Add(Resource.Coins, 100);
			_repository.Save();
			_repository.Load();
			var loadedState = _repository.State;
			Assert.True(loadedState.Resources.ContainsKey(Resource.Coins));
			Assert.Equal(100, loadedState.Resources[Resource.Coins]);
		}
	}
}