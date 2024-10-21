using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Common/GameConfigData", fileName = "GameConfig", order = 0)]
public class GameConfigData : ScriptableObject
{
	public Level[] levels;

	[Serializable]
	public struct Level
	{
		public int start;
		public int end;

		[Space]
		public int points;

		[Space]
		public Energy energy;

		[Space]
		public SlotConfigData slotConfig;
		public Match3ConfigData match3Config;

		[Space]
		public ShopConfigData shopConfig;

		[Space]
		public RewardData[] levelUpReward;
	}

	[Serializable]
	public struct Energy
	{
		public int max;

		public RewardData recoveryReward;
		public float recoveryCooldown;
	}
}