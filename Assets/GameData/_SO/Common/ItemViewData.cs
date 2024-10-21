using UnityEngine;

[CreateAssetMenu(menuName = "Game/Common/ItemViewData", fileName = "ItemView", order = 12)]
public class ItemViewData : ScriptableObject
{
	public int id;
	public string itemName;

	public Sprite icon;
}