using N.Fridman.FormatNums.Scripts.Helpers;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIItemPanel : MonoBehaviour, IUIStats, IPointerClickHandler
{
	public Action onClick;

	[SerializeField]
	private UIImageEffect m_icon;

	[SerializeField]
	private TMP_Text m_amountText;

	[Space]
	[SerializeField]
	private UIFloatingStringEffect m_floatingString;
	[SerializeField]
	private RectTransform m_dropRectTransform;

	private StatsBehaviour _stats;
	private UIEventsService _uiEvenetsService;

	private int _amount;

	private void Awake()
	{
		_stats = App.Instance.Gameplay.Stats;
		_uiEvenetsService = App.Instance.Services.Get<UIEventsService>();
	}

	public void SetAmount(int amount)
	{
		_amount = amount;

		m_amountText.text = FormatNumsHelper.FormatNum((float)_amount);
	}

	public void Add(int count)
	{
		m_icon.FlashScaleUpEffect();
		m_floatingString.FloatingStringEffect($"+{FormatNumsHelper.FormatNum((float)count)}", Color.green);

		SetAmount(_amount + count);
	}
	public void Take(int count)
	{
		m_icon.ScaleDownEffect();
		m_floatingString.FloatingStringEffect($"-{FormatNumsHelper.FormatNum((float)count)}", Color.red);

		SetAmount(_amount - count);
	}

	public RectTransform GetRectTransformCollecting()
	{
		return m_dropRectTransform;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		onClick?.Invoke();
		_uiEvenetsService.ShopOpen?.Invoke();
	}
}