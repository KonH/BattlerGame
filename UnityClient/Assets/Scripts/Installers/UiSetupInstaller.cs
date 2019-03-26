using UnityClient.Models;
using UnityClient.ViewModels;
using Zenject;

namespace UnityClient.Installers {
	public class UiSetupInstaller : ScriptableObjectInstaller {
		public NoticeViewModel NoticePrefab;
		
		public override void InstallBindings() {
			Container.BindFactory<NoticeModel, NoticeViewModel, NoticeViewModel.Factory>().FromComponentInNewPrefab(NoticePrefab);
		}
	}
}
