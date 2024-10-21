using UnityEngine;

[CreateAssetMenu(menuName = "Game/Slot/SlotReelItemData", fileName = "SlotReelItem", order = 32)]
public class SlotReelItemData : ScriptableObject
{
	public int index;
	public ItemViewData view;
	public RewardData[] reward;
}