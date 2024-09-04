using System;

namespace Game.Profile
{
	[Serializable]
	public class PlayerData : IProfileData
	{
		public Stats stats;
		public Progress progress;
		public EnergyRecovery energyRecovery;
		public Match3Cooldown match3Cooldown;

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
		}

		[Serializable]
		public class Stats
		{
			public int energy;
			public int coins;
			public int oil;
			public int scrubs;
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
			public DateTime startTime;
			public DateTime endTime;
		}

		[Serializable]
		public class Match3Cooldown
		{
			public DateTime startTime;
			public DateTime endTime;
		}
	}
}