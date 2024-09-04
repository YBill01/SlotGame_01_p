using UnityEngine;

[CreateAssetMenu(menuName = "Game/Common/ItemData", fileName = "ItemData", order = 1)]
public class ItemData : ScriptableObject
{
	public ItemType type;
	public ItemRate rate;

	public ItemViewData view;
}