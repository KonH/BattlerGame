using GameLogics.Shared.Models.Configs;
using UnityEditor;
using UnityEngine;

namespace UnityClient.Editor.ConfigEditor {
	class LevelEditor : DictEditor<LevelConfig> {
		public Config Context = null;
		
		UnitDescEditor _enemiesEditor = new UnitDescEditor();
		RewardEditor   _rewardEditor  = new RewardEditor();

		protected override LevelConfig New() => new LevelConfig();

		protected override void Update(LevelConfig level) {
			_enemiesEditor.Context = Context;
			_enemiesEditor.Update("Enemies", level.EnemyDescriptors);
			EditorGUILayout.Separator();

			GUILayout.Label("Reward");
			_rewardEditor.Context = Context;
			_rewardEditor.Update(level.Reward);
		}
	}
}