using System;
using UnityEngine;

public class UIShopItemsPanel : MonoBehaviour
{
	public Action<int, IUIStatsDrop> onShopPurchase;

	[SerializeField]
	private UIShopItem m_prefabItem;

	private ShopConfigData _data;

	private UIShopItem[] _items;

	public void SetData(ShopConfigData shopData)
	{
		_data = shopData;
		_items = new UIShopItem[_data.shopItems.Length];
	}

	public void CreateItems()
	{
		for (int i = 0; i < _data.shopItems.Length; i++)
		{
			UIShopItem item = Instantiate(m_prefabItem, transform);

			item.SetData(i, _data.shopItems[i]);
			item.onShopPurchase += ShopPurchase;

			_items[i] = item;
		}
	}
	public void ClearItems()
	{
		for (int i = 0; i < _items.Length; i++)
		{
			_items[i].onShopPurchase -= ShopPurchase;
			Destroy(_items[i].gameObject);
			_items[i] = null;
		}
	}

	private void ShopPurchase(int shopItemIndex)
	{
		onShopPurchase?.Invoke(shopItemIndex, _items[shopItemIndex]);
	}
}