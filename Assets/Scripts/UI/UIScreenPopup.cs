using DG.Tweening;
using System;
using UnityEngine;

public abstract class UIScreenPopup : UIFadeScreen
{
	[Space]
	[SerializeField]
	private RectTransform m_contentRectTransform;

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void OnInit()
	{
		_fadeInDuration = 0.125f;
		_fadeOutDuration = 0.125f;
	}

	protected override void SetDefault()
	{
		base.SetDefault();

		m_contentRectTransform.anchoredPosition = Vector3.zero;
	}

	protected override void OnImmediate()
	{
		base.OnImmediate();

		m_contentRectTransform.DOComplete();
	}

	protected override void FadeIn(Action action = null)
	{
		m_contentRectTransform.DOAnchorPosY(0.0f, _fadeInDuration)
			.From(new Vector2(0.0f, -64.0f));

		base.FadeIn(action);
	}
	protected override void FadeOut(Action action = null)
	{
		m_contentRectTransform.DOAnchorPosY(-64.0f, _fadeOutDuration)
			.From(new Vector2(0.0f, 0.0f));

		base.FadeOut(action);
	}
}