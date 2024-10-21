using N.Fridman.FormatNums.Scripts.Helpers;
using SlotGame.Profile;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEnergyPanel : MonoBehaviour, IUIStats, IPointerClickHandler
{
	public Action onClick;

	[SerializeField]
	private UIImageEffect m_icon;
	[SerializeField]
	private Image m_indicatorLine;

	[SerializeField]
	private Scrollbar m_scrollbar;
	[SerializeField]
	private UIImageEffect m_scrollbarLine;

	[SerializeField]
	private TMP_Text m_amountText;

	[Space]
	[SerializeField]
	private UIFloatingStringEffect m_floatingString;
	[SerializeField]
	private RectTransform m_dropRectTransform;

	private PlayerData _playerData;
	private StatsBehaviour _stats;

	private int _amount;

	private void Awake()
	{
		_playerData = Profile.Instance.Get<PlayerData>().data;
		_stats = App.Instance.Gameplay.Stats;

		SetAmount(_playerData.stats.energy);
		SetRecoveryProgress(0.0f);
	}

	public void SetAmount(int amount)
	{
		_amount = Mathf.Min(amount, _stats.GetCurrentLevelData().energy.max);

		m_amountText.text = FormatNumsHelper.FormatNum((float)_amount);
		m_scrollbar.size = (float)_amount / _stats.GetCurrentLevelData().energy.max;

		if (!_stats.IsEnergyRecovery)
		{
			SetRecoveryProgress(0.0f);
		}
	}

	public void SetRecoveryProgress(float value)
	{
		m_indicatorLine.fillAmount = value;
	}

	public void Add(int count)
	{
		m_icon.FlashEffect();
		m_scrollbarLine.FlashEffect();

		m_floatingString.FloatingStringEffect($"+{FormatNumsHelper.FormatNum((float)count)}", Color.green);

		SetAmount(_amount + count);
	}
	public void Take(int count)
	{
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
	}
}