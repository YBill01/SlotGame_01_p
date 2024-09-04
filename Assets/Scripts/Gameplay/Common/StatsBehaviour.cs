using Game.Profile;
using System;

public class StatsBehaviour
{

	public PlayerData playerData;

	private SlotConfigData _slotConfig;

	private GameplayEventsService _gameplayEvents;

	private bool _isEnergyRecovery = true;
	public bool IsEnergyRecovery => _isEnergyRecovery;

	private bool _isMatch3Cooldown = true;
	public bool IsMatch3Cooldown => _isMatch3Cooldown;


	SlotConfigData.Level _currentLevelData;
	private bool _isCurrentLevelDataDirty = true;

	public StatsBehaviour(SlotConfigData slotConfig)
	{
		_slotConfig = slotConfig;




		playerData = Profile.Instance.Get<PlayerData>().data;

		_gameplayEvents = App.Instance.Services.Get<GameplayEventsService>();

		Init();
	}

	private void Init()
	{
		



	}

	public void Update(float deltaTime)
	{
		EnergyRecoveryUpdate();
		Match3CooldownUpdate();

		
	}

	private void SetEnergyRecovery()
	{
		SetEnergyRecovery(DateTime.UtcNow, DateTime.UtcNow.AddSeconds(GetCurrentLevelData().energy.recoveryCooldown));
	}
	private void SetEnergyRecovery(DateTime start, DateTime end)
	{
		playerData.energyRecovery.startTime = start;
		playerData.energyRecovery.endTime = end;

		_isEnergyRecovery = true;
	}
	private void EnergyRecoveryUpdate()
	{
		if (_isEnergyRecovery)
		{
			SlotConfigData.Level levelData = GetCurrentLevelData();

			if (playerData.stats.energy < levelData.energy.max)
			{
				TimeSpan timeSpan = DateTime.UtcNow - playerData.energyRecovery.endTime;
				if (timeSpan.TotalSeconds >= 0)
				{
					// compensation offline recovery...
					int count = (int)(timeSpan.TotalSeconds / levelData.energy.recoveryCooldown);
					playerData.stats.energy += levelData.energy.recoveryCount * count;

					SetEnergyRecovery(DateTime.UtcNow.AddSeconds(timeSpan.TotalSeconds % levelData.energy.recoveryCooldown), DateTime.UtcNow.AddSeconds(levelData.energy.recoveryCooldown));

					AddEnergy(levelData.energy.recoveryCount);
				}
			}
			else
			{
				_isEnergyRecovery = false;
			}
		}
	}



	private void Match3CooldownUpdate()
	{
		if (_isMatch3Cooldown)
		{
			
		}
	}



	public void AddEnergy(int count)
	{
		playerData.stats.energy = Math.Min(playerData.stats.energy + count, GetCurrentLevelData().energy.max);

		_gameplayEvents.StatsEnergyUpdate?.Invoke(count, true);
	}
	public void TakeEnergy(int count)
	{
		playerData.stats.energy = Math.Max(playerData.stats.energy - count, 0);

		_gameplayEvents.StatsEnergyUpdate?.Invoke(count, false);
	}


	public void AddReward(RewardData[] reward)
	{

	}


	public void SetNewLevel(int value)
	{
		//AddReward(GetCurrentLevelData().levelUpReward);

		playerData.progress.level = value;
		playerData.progress.points = 0;

		playerData.energyRecovery.startTime = default;
		playerData.energyRecovery.endTime = default;
		playerData.match3Cooldown.startTime = default;
		playerData.match3Cooldown.endTime = default;

		SlotConfigData.Level levelData = GetCurrentLevelData(true);

		playerData.stats.energy = levelData.energy.max;


		//event level up...
	}

	

	public SlotConfigData.Level GetCurrentLevelData(bool forced = false)
	{
		if (_isCurrentLevelDataDirty || forced)
		{
			_currentLevelData = default;

			for (int i = 0; i < _slotConfig.levels.Length; i++)
			{
				_currentLevelData = _slotConfig.levels[i];
				if (playerData.progress.level >= _currentLevelData.start && playerData.progress.level <= _currentLevelData.end)
				{
					break;
				}
			}

			_isCurrentLevelDataDirty = false;
		}

		return _currentLevelData;
	}
}