using DG.Tweening;
using System;
using UnityEngine;

public abstract class UIGameScreen : UIScreen
{
	[Space]
	[SerializeField]
	protected RectTransform m_contentRectTransform;
	[SerializeField]
	protected RectTransform m_uiRectTransform;
	[Space]
	[SerializeField]
	private RectTransform m_sizeTransform;

	[Space]
	[SerializeField]
	private int m_sfxOpenIndex;
	[SerializeField]
	private int m_sfxCloseIndex;

	protected float _fadeInDuration;
	protected float _fadeOutDuration;

	protected CanvasGroup _canvasGroup;
	protected CanvasGroup _uiCanvasGroup;

	protected virtual void Awake()
	{
		_canvasGroup = GetComponent<CanvasGroup>();
		_uiCanvasGroup = m_uiRectTransform.GetComponent<CanvasGroup>();

		SetDefault();
	}

	protected override void OnInit()
	{
		_fadeInDuration = 0.25f;
		_fadeOutDuration = 0.25f;
	}

	protected virtual void SetDefault()
	{
		_canvasGroup.alpha = 1.0f;
		_canvasGroup.blocksRaycasts = false;
		_uiCanvasGroup.alpha = 0.0f;
		_uiCanvasGroup.blocksRaycasts = true;
	}

	protected override void OnActivate(Action action)
	{
		OnPreActivate(action);
		FadeIn();

		PlaySFX(m_sfxOpenIndex);
	}
	protected override void OnDeActivate(Action action)
	{
		OnPreDeActivate();
		FadeOut(action);

		PlaySFX(m_sfxCloseIndex);
	}

	protected virtual void OnImmediate()
	{
		_canvasGroup.DOComplete();
	}

	protected virtual void FadeIn(Action action = null)
	{
		_canvasGroup.blocksRaycasts = false;

		m_contentRectTransform.DOAnchorPosX(0.0f, _fadeInDuration)
			.From(new Vector2((m_sizeTransform.rect.width / 2) + (m_contentRectTransform.rect.width / 2), 0.0f))
			.SetEase(Ease.OutBack);

		_uiCanvasGroup.DOFade(1.0f, _fadeInDuration)
			.From(0.0f)
			.SetEase(Ease.OutQuad)
			.OnComplete(() =>
			{
				FadeInComplete(action);
			});
	}
	protected virtual void FadeOut(Action action = null)
	{
		_canvasGroup.blocksRaycasts = false;

		m_contentRectTransform.DOAnchorPosX(-((m_sizeTransform.rect.width / 2) + m_contentRectTransform.rect.width), _fadeInDuration)
			.From(new Vector2(0.0f, 0.0f))
			.SetEase(Ease.InQuart);

		_uiCanvasGroup.DOFade(0.0f, _fadeOutDuration)
			.From(1.0f)
			.SetEase(Ease.OutQuad)
			.OnComplete(() =>
			{
				FadeOutComplete(action);
			});
	}

	protected virtual void FadeInComplete(Action action = null)
	{
		_canvasGroup.blocksRaycasts = true;

		OnPostActivate(action);
	}
	protected virtual void FadeOutComplete(Action action = null)
	{
		OnPostDeActivate(action);
		SetDefault();
	}

	public void PlaySFX(int index)
	{
		if (index < 0)
		{
			return;
		}

		App.Instance.Services
			.Get<UIService>()
			.Get<UISoundService>()
			.PlaySFXOnceShot(index);
	}
}