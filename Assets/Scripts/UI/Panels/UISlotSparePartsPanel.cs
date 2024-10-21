using SlotGame.Profile;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISlotSparePartsPanel : MonoBehaviour
{
	[Space]
	[SerializeField]
	private TMP_Text m_priceText;

	[Space]
	[SerializeField]
	private Button m_plusButton;
	[SerializeField]
	private Button m_minusButton;

	[Space]
	[SerializeField]
	private TMP_Text m_multiplierText;

	private PlayerData _playerData;
	private SlotConfigData _data;

	public void SetData(SlotConfigData data, PlayerData playerData)
	{
		_data = data;
		_playerData = playerData;

		SetState();
	}

	private void OnEnable()
	{
		m_plusButton.onClick.AddListener(PlusButtonOnClick);
		m_minusButton.onClick.AddListener(MinusButtonOnClick);
	}
	private void OnDisable()
	{
		m_plusButton.onClick.RemoveListener(PlusButtonOnClick);
		m_minusButton.onClick.RemoveListener(MinusButtonOnClick);
	}

	private void SetState()
	{
		int priceAmount = 0;
		foreach (RewardData price in _data.spinPrice)
		{
			if (price.item.type == ItemType.SpareParts)
			{
				priceAmount += price.count * _playerData.slotProperties.sparePartsMultiplier;
			}
		}

		m_priceText.text = $"-{priceAmount}";

		m_multiplierText.text = $"<size=42>x</size>{_playerData.slotProperties.sparePartsMultiplier}";
	}

	private void PlusButtonOnClick()
	{
		_playerData.slotProperties.sparePartsMultiplier++;

		SetState();
	}
	private void MinusButtonOnClick()
	{
		_playerData.slotProperties.sparePartsMultiplier = Mathf.Max(1, _playerData.slotProperties.sparePartsMultiplier - 1);

		SetState();
	}
}