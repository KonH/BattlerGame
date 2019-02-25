using UnityEditor;

namespace UnityClient.Editor {
	public static class BuildMenu {
		static BuildUtility _utility = new BuildUtility("GameLogics", "Assets/Plugins/GameLogics/netstandard2.0");

		[MenuItem("Build/Configuration/Debug")]
		public static void Configuration_Debug() {
			_utility.BuildForConfiguration("Debug");
		}

		[MenuItem("Build/Configuration/Release")]
		public static void Configuration_Release() {
			_utility.BuildForConfiguration("Release");
		}
	}
}
