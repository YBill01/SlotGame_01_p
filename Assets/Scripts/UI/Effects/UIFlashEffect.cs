using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(Image))]
public class UIFlashEffect : MonoBehaviour
{
	[SerializeField]
	private float m_duration = 1.0f;

	private Image _image;
	private Material _material;

	private void Awake()
	{
		_image = GetComponent<Image>();
	}

	[ContextMenu("FlashEffect")]
	public void FlashEffect()
	{
		_material = _image.material;
		_image.material = new Material(_material);

		float value = 0.0f;
		DOTween.To(() => value, x => value = x, 180.0f, m_duration)
			.SetEase(Ease.InOutQuad)
			.OnUpdate(() => {
				float valueResult = Mathf.Sin(value * Mathf.Deg2Rad);
				_image.material.SetFloat("_FlashAmount", valueResult * 0.75f);
			})
			.OnComplete(() => {
				_image.material = _material;
			});
	}

	[ContextMenu("FlashScaleEffect")]
	public void FlashScaleEffect()
	{
		_material = _image.material;
		_image.material = new Material(_material);

		float value = 0.0f;
		DOTween.To(() => value, x => value = x, 180.0f, m_duration)
			.SetEase(Ease.InOutQuad)
			.OnUpdate(() => {
				float valueResult = Mathf.Sin(value * Mathf.Deg2Rad);
				_image.material.SetFloat("_FlashAmount", valueResult * 0.75f);
				_image.rectTransform.localScale = new Vector3(1.0f + (valueResult * 0.25f), 1.0f + (valueResult * 0.25f), 1.0f);
			})
			.OnComplete(() => {
				_image.material = _material;
			});
	}
}