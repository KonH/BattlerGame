using System;
using System.IO;
using GameLogics.Core;
using GameLogics.Managers;
using Xunit;

namespace UnitTests {
	public class LocalGameStateManagerTest : IDisposable {
		const string _path = "file.json";
		
		LocalGameStateManager _manager = new LocalGameStateManager(_path);
		
		public void Dispose() {
			if ( File.Exists(_path) ) {
				File.Delete(_path);
			}
		}
		
		[Fact]
		public void FileIsSaved() {
			_manager.Load();
			_manager.Save();
			Assert.True(File.Exists(_path));
		}

		[Fact]
		public void ResourceIsSaved() {
			_manager.Load();
			_manager.State.Resources.Add(Resource.Coins, 100);
			_manager.Save();
			_manager.Load();
			var loadedState = _manager.State;
			Assert.True(loadedState.Resources.ContainsKey(Resource.Coins));
			Assert.Equal(100, loadedState.Resources[Resource.Coins]);
		}
	}
}