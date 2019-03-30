using UnityEditor;
using UnityClient.Utils;

namespace UnityClient.Editor.ConfigEditor {	
	class FeatureEditor : DictEditor<Boxed<bool>> {
		protected override Boxed<bool> New() => new Boxed<bool>(false);

		protected override void Update(Boxed<bool> feature) {
			feature.Value = EditorGUILayout.Toggle(feature.Value);
		}
	}
}