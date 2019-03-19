using GameLogics.Shared.Models.Configs;
using UnityEditor;

namespace UnityClient.Editor.ConfigEditor {
	class UnitEditor : DictEditor<UnitConfig> {
		protected override UnitConfig New() => new UnitConfig(1);

		protected override void Update(UnitConfig unit) {
			unit.BaseDamage = EditorGUILayout.IntField("BaseDamage:", unit.BaseDamage);
		}
	}
}