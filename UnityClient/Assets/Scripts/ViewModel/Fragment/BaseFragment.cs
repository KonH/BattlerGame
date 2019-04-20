using UnityEngine;

namespace UnityClient.ViewModel.Fragment {
	public abstract class BaseFragment : MonoBehaviour {
		protected void Attach(Transform parent) {
			transform.SetParent(parent, false);
		}
	}
}