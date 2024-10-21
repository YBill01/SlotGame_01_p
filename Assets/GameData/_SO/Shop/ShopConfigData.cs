using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Shop/ShopConfigData", fileName = "ShopConfig", order = 12)]
public class ShopConfigData : ScriptableObject
{
	public ShopItem[] shopItems;

	[Serializable]
	public struct ShopItem
	{
		public ItemViewData view;
		public Color color;

		[Space]
		public string name;

		[Space]
		public RewardData[] price;
		public RewardData[] reward;
	}
}