using UnityEngine;
using UnityClient.Utils;

namespace UnityClient.ViewModels.Windows.Animations {
	public class ScaleAnimation : BaseAnimation {
		protected override AsyncExtensions.UpdateHelper PerformShow() {
			Root.localScale = Vector3.zero;
			return Root.DoScale(ShowDuration, Vector3.one);
		}

		protected override AsyncExtensions.UpdateHelper PerformHide() {
			return Root.DoScale(HideDuration, Vector3.zero);
		}
	}
}