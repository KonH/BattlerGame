using System.Diagnostics;
using System.IO;
using UnityClient.Installers;
using UnityEditor;
using UnityEngine;

namespace UnityClient.Editor {
	public static class UtilityMenu {
		[MenuItem("Utils/Delete State")]
		public static void DeleteState() {
			if ( Directory.Exists(Application.persistentDataPath) ) {
				var files = Directory.EnumerateFiles(Application.persistentDataPath, "*.json");
				foreach ( var file in files ) {
					File.Delete(file);
				}
			}
		}
		
		[MenuItem("Utils/Open State")]
		public static void OpenState() {
			Process.Start(Application.persistentDataPath);
		}
		
		[MenuItem("Utils/Create/Fragment Installer")]
		public static void CreateFragmentInstaller() {
			CreateInstaller<FragmentInstaller>();
		}

		static void CreateInstaller<T>() where T : ScriptableObject {
			var instance         = ScriptableObject.CreateInstance<T>();
			var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"Assets/Installers/{typeof(T).Name}.asset");
			AssetDatabase.CreateAsset(instance, assetPathAndName);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
	}
}
