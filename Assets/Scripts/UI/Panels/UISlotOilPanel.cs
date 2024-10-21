using SlotGame.Profile;
using TMPro;
using UnityEngine;

public class UISlotOilPanel : MonoBehaviour
{
	[SerializeField]
	private UIToggleComponent m_useOilToggle;
	[Space]
	[SerializeField]
	private TMP_Text m_priceText;
	[Space]
	[SerializeField]
	private TMP_Text m_useOilToggleOnText;

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
		m_useOilToggle.onValueChanged += UseOilToggleOnValueChanged;
	}
	private void OnDisable()
	{
		m_useOilToggle.onValueChanged += UseOilToggleOnValueChanged;
	}

	private void SetState()
	{
		int priceAmount = 0;
		foreach (RewardData price in _data.oilBelay.price)
		{
			if (price.item.type == ItemType.Oil)
			{
				priceAmount += price.count;
			}
		}

		m_priceText.text = _playerData.slotProperties.useOilBelay ? $"-{priceAmount}" : "0";

		m_useOilToggle.Value = _playerData.slotProperties.useOilBelay;
		m_useOilToggleOnText.text = $"return up to {_data.oilBelay.rewardMaxPercent:P0}";
	}

	private void UseOilToggleOnValueChanged(bool value)
	{
		_playerData.slotProperties.useOilBelay = value;

		SetState();
	}
}