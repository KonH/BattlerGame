using GameLogics.Shared.Models.Configs;

namespace UnityClient.Editor.ConfigEditor {
	public class RewardEditor {
		public Config Context = null;
		
		ResourceEditor _resEditor  = new ResourceEditor();
		ItemDescEditor _itemEditor = new ItemDescEditor();
		UnitDescEditor _unitEditor = new UnitDescEditor();

		public void Update(RewardConfig reward) {
			_resEditor.Context = Context;
			_resEditor.Update("Resources", reward.Resources);
			
			_itemEditor.Context = Context;
			_itemEditor.Update("Items", reward.Items);
			
			_unitEditor.Context = Context;
			_unitEditor.Update("Units", reward.Units);
		}
	}
}