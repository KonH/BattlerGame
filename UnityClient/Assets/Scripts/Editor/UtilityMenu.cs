using System.Collections;
using System.Collections.Generic;
using System.IO;
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
	}
}
