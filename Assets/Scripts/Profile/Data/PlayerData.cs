using System;

namespace SlotGame.Profile
{
	[Serializable]
	public class PlayerData : IProfileData
	{
		public Stats stats;
		public Progress progress;
		public EnergyRecovery energyRecovery;
		public Match3Cooldown match3Cooldown;
		public SlotProperties slotProperties;

		public PlayerData()
		{
			SetDefault();
		}

		public void SetDefault()
		{
			stats = new Stats();
			progress = new Progress();
			energyRecovery = new EnergyRecovery();
			match3Cooldown = new Match3Cooldown();
			slotProperties = new SlotProperties();
		}

		[Serializable]
		public class Stats
		{
			public int energy;
			public int coins;
			public int oil;
			public int spareParts;
		}

		[Serializable]
		public class Progress
		{
			public int level;
			public int points;
		}
		
		[Serializable]
		public class EnergyRecovery
		{
			public DateTime endTime;
		}

		[Serializable]
		public class Match3Cooldown
		{
			public DateTime endTime;
		}

		[Serializable]
		public class SlotProperties
		{
			public bool useOilBelay;
			public int sparePartsMultiplier;
		}
	}
}