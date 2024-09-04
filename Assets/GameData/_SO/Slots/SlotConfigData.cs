using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Slot/SlotConfigData", fileName = "SlotConfig", order = 10)]
public class SlotConfigData : ScriptableObject
{
	public Level[] levels;

	[Space(10)]
	public float scrollTime = 2.0f;
	public int scrollCount = 16;

	[Serializable]
	public struct Level
	{
		public int start;
		public int end;

		public int points;

		public Energy energy;

		public SlotPatternData pattern;
		public SlotReelData reel;

		public RewardData[] levelUpReward;
	}

	[Serializable]
	public struct Energy
	{
		public int max;

		public int recoveryCount;
		public float recoveryCooldown;
	}
}