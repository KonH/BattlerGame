using UnityEngine;
using UnityClient.Utils;

namespace UnityClient.ViewModels.Windows.Animations {
	public class ScaleAnimation : BaseAnimation {
		public override AsyncExtensions.UpdateHelper PerformShow() {
			Root.localScale = Vector3.zero;
			return Root.DoScale(ShowDuration, Vector3.one);
		}

		public override AsyncExtensions.UpdateHelper PerformHide() {
			return Root.DoScale(HideDuration, Vector3.zero);
		}
	}
}