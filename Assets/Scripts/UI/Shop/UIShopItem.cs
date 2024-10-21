using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIShopItem : MonoBehaviour, IUIStatsDrop
{
	public Action<int> onShopPurchase;

	[SerializeField]
	private UIImageEffect m_image;
	[SerializeField]
	private TMP_Text m_rewardText;
	[SerializeField]
	private TMP_Text m_priceText;

	[Space]
	[SerializeField]
	private RectTransform m_dropRectTransform;

	private int _itemIndex;
	private ShopConfigData.ShopItem _data;

	private Button _button;

	private void Awake()
	{
		_button = GetComponent<Button>();
	}

	private void OnEnable()
	{
		_button.onClick.AddListener(OnClick);
	}
	private void OnDisable()
	{
		_button.onClick.RemoveListener(OnClick);
	}

	public void SetData(int index, ShopConfigData.ShopItem data)
	{
		_itemIndex = index;
		_data = data;

		m_image.GetComponent<Image>().sprite = _data.view.icon;
		m_image.GetComponent<Image>().color = _data.color;

		int rewardAmount = 0;
		foreach (RewardData reward in _data.reward)
		{
			rewardAmount += reward.count;
		}

		m_rewardText.text = $"<size=42>x</size>{rewardAmount}";
		m_rewardText.color = _data.color;

		int priceAmount = 0;
		foreach (RewardData price in _data.price)
		{
			priceAmount += price.count;
		}

		m_priceText.text = $"<size=24>-</size>{priceAmount}";
	}

	public RectTransform GetRectTransformCollecting()
	{
		return m_dropRectTransform;
	}

	public void Buyed()
	{
		m_image.FlashScaleUpEffect();
	}

	private void OnClick()
	{
		onShopPurchase?.Invoke(_itemIndex);
	}
}