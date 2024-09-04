using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UICurtainScreen : UIFadeScreen
{
	[Space]
	[SerializeField]
	private RectTransform bgMaskTransform;
	//[SerializeField]
	//private RectTransform bgTransform;
	[SerializeField]
	private RectTransform bgMaskShadowTransform;
	[SerializeField]
	private RectTransform bgSizeTransform;

	private Vector2 _startBgMaskPosition;
	private Rect _startBgMaskRect;
	private Vector2 _startBgMaskShadowPosition;
	private Rect _startBgMaskShadowRect;
	private Rect _sizeRect;
	private Vector2 _inBgMaskPosition;
	private Vector2 _outBgMaskPosition;

	protected override void SetDefault()
	{
		base.SetDefault();

		_canvasGroup.alpha = 1.0f;
		_canvasGroup.blocksRaycasts = true;

		_startBgMaskPosition = bgMaskTransform.anchoredPosition;
		_startBgMaskRect = bgMaskTransform.rect;
		_startBgMaskShadowPosition = bgMaskShadowTransform.anchoredPosition;
		_startBgMaskShadowRect = bgMaskShadowTransform.rect;
		_sizeRect = bgSizeTransform.rect;
		
		_inBgMaskPosition = new Vector2(0.0f, -_sizeRect.height);
		_outBgMaskPosition = new Vector2(0.0f, _startBgMaskRect.height);

		//bgMaskTransform.anchoredPosition = _inBgMaskPosition;
		//bgMaskTransform.anchoredPosition = _outBgMaskPosition;
		//bgMaskTransform.sizeDelta = new Vector2(_startBgMaskRect.width, _startBgMaskRect.height);
	}

	protected override void FadeIn(Action action = null)
	{
		bgMaskTransform.sizeDelta = _startBgMaskRect.size;
		bgMaskShadowTransform.sizeDelta = _startBgMaskShadowRect.size;
		//bgTransform.anchoredPosition = _startBgMaskPosition - _inBgMaskPosition;

		bgMaskTransform.DOAnchorPos(_outBgMaskPosition, _fadeInDuration)
			.From(_inBgMaskPosition)
			.SetEase(Ease.OutQuad)
			.OnUpdate(() =>
			{
				//bgTransform.anchoredPosition = _startBgMaskPosition - bgMaskTransform.anchoredPosition;
				//bgMaskTransform.sizeDelta = _inBgMaskPosition - bgMaskTransform.anchoredPosition + _startBgMaskRect.size;
				bgMaskShadowTransform.anchoredPosition = bgMaskTransform.anchoredPosition + ((_startBgMaskShadowRect.size - _startBgMaskRect.size) / 2);
				bgMaskShadowTransform.sizeDelta = bgMaskTransform.anchoredPosition - _inBgMaskPosition + _startBgMaskShadowRect.size;
				bgMaskTransform.sizeDelta = bgMaskTransform.anchoredPosition - _inBgMaskPosition + _startBgMaskRect.size;
			})
			.OnComplete(() =>
			{
				OnPostActivate(action);
			});
	}
	protected override void FadeOut(Action action = null)
	{
		bgMaskTransform.anchoredPosition = _outBgMaskPosition;
		//bgTransform.anchoredPosition = _startBgMaskPosition - _outBgMaskPosition;

		bgMaskTransform.DOSizeDelta(_startBgMaskRect.size, _fadeOutDuration)
			.From(_outBgMaskPosition - _inBgMaskPosition + _startBgMaskRect.size)
			.SetEase(Ease.OutQuad)
			.OnUpdate(() =>
			{
				bgMaskShadowTransform.sizeDelta = bgMaskTransform.sizeDelta + (_startBgMaskShadowRect.size - _startBgMaskRect.size);
			})
			.OnComplete(() => 
			{
				OnPostDeActivate(action);
				SetDefault();
			});
	}
}