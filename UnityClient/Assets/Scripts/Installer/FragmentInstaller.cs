using UnityClient.Model;
using UnityClient.ViewModel.Fragment;
using UnityEngine;
using Zenject;

namespace UnityClient.Installer {
	public sealed class FragmentInstaller : ScriptableObjectInstaller {
		public ItemFragment   ItemFragmentPrefab;
		public UnitFragment   UnitFragmentPrefab;
		public RewardFragment RewardFragmentPrefab;
		
		public override void InstallBindings() {
			Container.BindFactory<Transform, ItemModel, ItemFragment, ItemFragment.Factory>().FromComponentInNewPrefab(ItemFragmentPrefab);
			Container.BindFactory<Transform, UnitModel, UnitFragment, UnitFragment.Factory>().FromComponentInNewPrefab(UnitFragmentPrefab);
			Container.BindFactory<Transform, string, RewardFragment, RewardFragment.Factory>().FromComponentInNewPrefab(RewardFragmentPrefab);
		}
	}

}
