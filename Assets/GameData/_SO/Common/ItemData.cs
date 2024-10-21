using UnityEngine;

[CreateAssetMenu(menuName = "Game/Common/ItemData", fileName = "Item", order = 11)]
public class ItemData : ScriptableObject
{
	public ItemType type;
	public ItemViewData view;
}