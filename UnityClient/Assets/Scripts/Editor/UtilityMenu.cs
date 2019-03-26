using System.Collections;
using System.Collections.Generic;
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

		[MenuItem("Utils/Create/UI Setup Installer")]
		public static void CreateUiSetupInstaller() {
			var instance = ScriptableObject.CreateInstance<UiSetupInstaller>();
			var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"Assets/Installers/{typeof(UiSetupInstaller).Name}.asset");
			AssetDatabase.CreateAsset(instance, assetPathAndName);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
	}
}
