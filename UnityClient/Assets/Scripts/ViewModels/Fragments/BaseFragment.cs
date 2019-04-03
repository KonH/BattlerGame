using UnityEngine;

namespace UnityClient.ViewModels.Fragments {
	public abstract class BaseFragment : MonoBehaviour {
		protected void Attach(Transform parent) {
			transform.SetParent(parent, false);
		}
	}
}