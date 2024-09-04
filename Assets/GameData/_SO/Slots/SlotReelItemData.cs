using UnityEngine;

[CreateAssetMenu(menuName = "Game/Slot/SlotReelItemData", fileName = "SlotReelItem", order = 32)]
public class SlotReelItemData : ScriptableObject
{
	public ItemData data;

	public float percent;

	public RewardData[] reward;
}