using UnityEngine;

public class UIShopScreen : UIPopup2Screen
{
	[Space]
	[SerializeField]
	private UIShopItemsPanel m_itemsPanel;

	[Space]
	[SerializeField]
	private int m_sfxOpenIndex;
	[SerializeField]
	private int m_sfxCloseIndex;

	private ShopConfigData _config;

	private void OnEnable()
	{
		m_itemsPanel.onShopPurchase += ShopPurchase;
	}
	private void OnDisable()
	{
		m_itemsPanel.onShopPurchase -= ShopPurchase;
	}

	protected override void OnInit()
	{
		base.OnInit();

		_config = App.Instance.Gameplay.Stats.GetCurrentLevelData().shopConfig;

		m_itemsPanel.SetData(_config);
		m_itemsPanel.CreateItems();
	}

	protected override void OnShow()
	{
		PlaySFX(m_sfxOpenIndex);
	}

	protected override void OnHide()
	{
		m_itemsPanel.ClearItems();

		PlaySFX(m_sfxCloseIndex);
	}

	public void BackButtonOnDown()
	{
		SelfHideInternal();
	}

	private void ShopPurchase(int shopItemIndex, IUIStatsDrop target)
	{
		if (App.Instance.Gameplay.Stats.ShopPurchase(shopItemIndex, (reward) =>
		{
			// TODO maybe NOT! UIGameplay in services...
			App.Instance.Services
				.Get<UIService>()
				.Get<UIGameplay>()
				.statsBehaviour
				.AddReward(reward, target.GetRectTransformCollecting());
		}))
		{
			(target as UIShopItem).Buyed();

			//Debug.Log($"shop purchase item: {shopItemIndex}");
		}
	}

	public void PlaySFX(int index)
	{
		App.Instance.Services
			.Get<UIService>()
			.Get<UISoundService>()
			.PlaySFXOnceShot(index);
	}
}