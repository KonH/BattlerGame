using GameLogics.Shared.Models.Configs;
using UnityEditor;

namespace UnityClient.Editor.ConfigEditor {
	class LevelEditor : DictEditor<LevelConfig> {
		public Config Context = null;
		
		UnitDescEditor _unitDescsEditor = new UnitDescEditor(); 
		
		protected override void Update(LevelConfig level) {
			_unitDescsEditor.Context = Context;
			_unitDescsEditor.Update("Enemies", level.EnemyDescriptors);
			EditorGUILayout.Separator();
		}
	}
}