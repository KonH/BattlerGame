using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using UnityEditor;

namespace UnityClient.Editor.ConfigEditor {
	class ItemEditor : DictEditor<ItemConfig> {
		protected override void Update(ItemConfig item) {
			item.Type = (ItemType)EditorGUILayout.EnumPopup("Type", item.Type);
		}
	}
}