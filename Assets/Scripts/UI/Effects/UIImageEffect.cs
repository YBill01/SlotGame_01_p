using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(Image))]
public class UIImageEffect : MonoBehaviour
{
	[SerializeField]
	private float m_duration = 1.0f;

	[SerializeField]
	private float m_flashFactor = 1.0f;

	private Image _image;
	private Material _material;

	private Tween _tween;

	private void Awake()
	{
		_image = GetComponent<Image>();
	}

	public void FlashEffect()
	{
		_material = _image.material;
		_image.material = new Material(_material);

		float value = 0.0f;
		DOTween.To(() => value, x => value = x, 180.0f, m_duration)
			.SetEase(Ease.InOutQuad)
			.OnUpdate(() => {
				float valueResult = Mathf.Sin(value * Mathf.Deg2Rad);
				_image.material.SetFloat("_FlashAmount", valueResult * m_flashFactor);
			})
			.OnComplete(() => {
				_image.material = _material;
			});
	}

	public void FlashScaleUpEffect()
	{
		_material = _image.material;
		_image.material = new Material(_material);

		float value = 0.0f;
		DOTween.To(() => value, x => value = x, 180.0f, m_duration)
			.SetEase(Ease.InOutQuad)
			.OnUpdate(() => {
				float valueResult = Mathf.Sin(value * Mathf.Deg2Rad);
				_image.material.SetFloat("_FlashAmount", valueResult * m_flashFactor);
				_image.rectTransform.localScale = new Vector3(1.0f + (valueResult * 0.25f), 1.0f + (valueResult * 0.25f), 1.0f);
			})
			.OnComplete(() => {
				_image.material = _material;
			});
	}
	public void FlashScaleDownEffect()
	{
		_material = _image.material;
		_image.material = new Material(_material);

		float value = 0.0f;
		DOTween.To(() => value, x => value = x, 180.0f, m_duration)
			.SetEase(Ease.InOutQuad)
			.OnUpdate(() => {
				float valueResult = Mathf.Sin(value * Mathf.Deg2Rad);
				_image.material.SetFloat("_FlashAmount", valueResult * m_flashFactor);
				_image.rectTransform.localScale = new Vector3(1.0f - (valueResult * 0.25f), 1.0f - (valueResult * 0.25f), 1.0f);
			})
			.OnComplete(() => {
				_image.material = _material;
			});
	}

	public void FlashScaleInEffect()
	{
		_material = _image.material;
		_image.material = new Material(_material);

		float value = 0.0f;
		DOTween.To(() => value, x => value = x, 180.0f, m_duration)
			.SetEase(Ease.OutQuad)
			.OnUpdate(() => {
				float valueResult = Mathf.Sin(value * Mathf.Deg2Rad);
				float valueResult2 = value / 180.0f;
				_image.material.SetFloat("_FlashAmount", valueResult * m_flashFactor);
				_image.rectTransform.localScale = new Vector3(0.1f + (valueResult2 * 0.9f), 0.1f + (valueResult2 * 0.9f), 1.0f);
			})
			.OnComplete(() => {
				_image.material = _material;
			});
	}

	public void ScaleDownEffect()
	{
		float value = 0.0f;
		DOTween.To(() => value, x => value = x, 180.0f, m_duration)
			.SetEase(Ease.InOutQuad)
			.OnUpdate(() => {
				float valueResult = Mathf.Sin(value * Mathf.Deg2Rad);
				_image.rectTransform.localScale = new Vector3(1.0f - (valueResult * 0.25f), 1.0f - (valueResult * 0.25f), 1.0f);
			});
	}

	public void ScaleInEffect()
	{
		_tween?.Complete();
		_tween = _image.rectTransform.DOScale(1.25f, m_duration)
			.SetEase(Ease.OutBack);
	}
	public void ScaleOutEffect()
	{
		_tween?.Complete();
		_tween = _image.rectTransform.DOScale(1.0f, m_duration)
			.SetEase(Ease.OutQuad);
	}

	public void ShakeEffect()
	{
		_tween?.Complete();
		_tween = _image.rectTransform.DOShakeAnchorPos(m_duration, new Vector2(24.0f, 0.0f), 16, 0)
			.OnComplete(() => {
				_image.rectTransform.anchoredPosition = Vector2.zero;
			});
	}

	public void AlphaInEffect()
	{
		_image.DOFade(1.0f, m_duration)
			.From(0.0f)
			.SetEase(Ease.OutQuad)
			/*.OnComplete(() => {
				
			})*/;
	}
	public void AlphaOutEffect(Action action)
	{
		_image.DOFade(0.0f, m_duration)
			.From(1.0f)
			.SetEase(Ease.InQuad)
			.OnComplete(() => {
				_tween?.Complete();
				action?.Invoke();
			});
	}

	private void OnDestroy()
	{
		_tween?.Kill();
	}
}