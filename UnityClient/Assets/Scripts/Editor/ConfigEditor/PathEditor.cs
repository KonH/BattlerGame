using System.Collections.Generic;
using UnityEditor;

namespace UnityClient.Editor.ConfigEditor {
	public class PathEditor : ListEditor<string> {
		protected override string New() => "";

		protected override void Update(List<string> items, int index) {
			items[index] = EditorGUILayout.TextField("Path:", items[index]);
		}
	}
}