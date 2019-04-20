using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityClient.Utils;
using Zenject;
using TMPro;

namespace UnityClient.View {
	public sealed class UnitView : MonoBehaviour {
		public sealed class Factory : PlaceholderFactory<UnitView> {}

		public Color PlayerHealthColor = Color.green;
		public Color EnemyHealthColor  = Color.red;
		public float DistanceCoeff     = 1.0f;
		public float MinScale          = 0.0f;
		public float MaxScale          = 1.0f;

		public Slider   HealthSlider = null;
		public Image    HealthImage  = null;
		public TMP_Text DamageText   = null;

		public void UpdateDistance(float distance) {
			transform.localScale = Vector3.one * Mathf.Clamp((1 / distance) * DistanceCoeff, MinScale, MaxScale);
		}

		public void SelectColor(bool isPlayerUnit) {
			HealthImage.color = isPlayerUnit ? PlayerHealthColor : EnemyHealthColor;
			DamageText.text = "";
		}

		public void UpdateHealth(float value) {
			HealthSlider.value = value;
		}

		public async Task AnimateDamage(int diff) {
			DamageText.text = (diff > 0) ? $"-{diff}" : "";
			var textTrans = DamageText.transform;
			textTrans.localScale = Vector3.zero;
			await textTrans.DoScale(0.25f, 1.0f);
			await textTrans.DoScale(0.5f, 0.0f);
		}
	}
}
