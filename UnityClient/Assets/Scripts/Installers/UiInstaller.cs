using UnityClient.Managers;
using UnityClient.Models;
using UnityClient.ViewModels;
using Zenject;

namespace UnityClient.Installers {
	public class UiInstaller : MonoInstaller {
		public UiManager.Settings Settings;
		public NoticeViewModel    NoticePrefab;
		
		public override void InstallBindings() {
			Container.BindInstance(Settings);
			Container.Bind(typeof(UiManager), typeof(ITickable)).To<UiManager>().AsSingle().NonLazy();
			Container.BindFactory<NoticeModel, NoticeViewModel, NoticeViewModel.Factory>().FromComponentInNewPrefab(NoticePrefab);
		}
	}
}