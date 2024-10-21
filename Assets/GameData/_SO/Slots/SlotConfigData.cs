using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Slot/SlotConfigData", fileName = "SlotConfig", order = 10)]
public class SlotConfigData : ScriptableObject
{
	public SlotPatternData pattern;
	public SlotReelData reel;

	[Space]
	public float rewardMultiplier;

	[Space]
	public RewardData[] spinPrice;

	[Space]
	public OilBelay oilBelay;

	[Space]
	public float scrollTime = 2.0f;
	public int scrollCount = 16;

	[Serializable]
	public struct OilBelay
	{
		public RewardData[] price;

		public float rewardMinPercent;
		public float rewardMaxPercent;
	}
}