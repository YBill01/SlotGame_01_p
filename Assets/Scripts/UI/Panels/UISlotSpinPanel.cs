using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISlotSpinPanel : MonoBehaviour
{
	public Action onSpin;

	[SerializeField]
	private Button m_spinButton;
	[Space]
	[SerializeField]
	private TMP_Text m_priceText;

	private SlotConfigData _data;

	private void OnEnable()
	{
		m_spinButton.onClick.AddListener(OnButtonOnClick);
	}
	private void OnDisable()
	{
		m_spinButton.onClick.RemoveListener(OnButtonOnClick);
	}

	public void SetData(SlotConfigData data)
	{
		_data = data;

		int priceAmount = 0;
		foreach (RewardData price in _data.spinPrice)
		{
			if (price.item.type == ItemType.Energy)
			{
				priceAmount += price.count;
			}
		}

		m_priceText.text = $"-{priceAmount}";
	}

	private void OnButtonOnClick()
	{
		onSpin?.Invoke();
	}
}