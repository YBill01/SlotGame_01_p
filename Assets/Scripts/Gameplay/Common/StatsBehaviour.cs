using SlotGame.Profile;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StatsBehaviour
{
	private GameConfigData _gameConfig;
	private GameStatsStartData _gameStatsStart;

	private PlayerData _playerData;

	private GameplayEventsService _gameplayEvents;

	private Dictionary<ItemType, Func<int, bool, bool, bool>> _playerStats;

	private bool _isEnergyRecovery = true;
	public bool IsEnergyRecovery => _isEnergyRecovery;

	private bool _isMatch3Cooldown = true;
	public bool IsMatch3Cooldown => _isMatch3Cooldown;

	private bool _isLevelUp = false;
	public bool IsLevelUp => _isLevelUp;

	GameConfigData.Level _currentLevelData;
	private bool _isCurrentLevelDataDirty = true;

	public StatsBehaviour(GameConfigData gameConfig, GameStatsStartData gameStatsStart)
	{
		_gameConfig = gameConfig;
		_gameStatsStart = gameStatsStart;

		_playerData = Profile.Instance.Get<PlayerData>().data;

		_gameplayEvents = App.Instance.Services.Get<GameplayEventsService>();

		SetPlayerStats();

		Init();
	}

	private void SetPlayerStats()
	{
		_playerStats = new Dictionary<ItemType, Func<int, bool, bool, bool>>
		{
			{ ItemType.Energy, StatsEnergy },
			{ ItemType.Points, StatsPoints },
			{ ItemType.SpareParts, StatsSpareParts },
			{ ItemType.Oil, StatsOil },
			{ ItemType.Coins, StatsCoins }
		};
	}

	private void Init()
	{
		if (Profile.Instance.Get<AppData>().data.firstPlay)
		{
			_playerData.progress.level = _gameStatsStart.level;
			_playerData.progress.points = _gameStatsStart.points;

			_playerData.stats.energy = Mathf.Min(_gameStatsStart.energy, GetCurrentLevelData().energy.max);
			_playerData.stats.coins = _gameStatsStart.coins;
			_playerData.stats.oil = _gameStatsStart.oil;
			_playerData.stats.spareParts = _gameStatsStart.spareParts;

			_playerData.slotProperties.useOilBelay = true;
			_playerData.slotProperties.sparePartsMultiplier = 1;

			Profile.Instance.Get<AppData>().data.firstPlay = false;
		}
	}

	public void Update()
	{
		EnergyRecoveryUpdate();
		Match3CooldownUpdate();
	}

	private void SetEnergyRecovery()
	{
		if (!_isEnergyRecovery)
		{
			SetEnergyRecovery(DateTime.UtcNow.AddSeconds(GetCurrentLevelData().energy.recoveryCooldown));
		}
	}
	private void SetEnergyRecovery(DateTime end)
	{
		_playerData.energyRecovery.endTime = end;

		_isEnergyRecovery = true;
	}
	private void EnergyRecoveryUpdate()
	{
		if (_isEnergyRecovery)
		{
			GameConfigData.Level levelData = GetCurrentLevelData();

			if (_playerData.stats.energy < levelData.energy.max)
			{
				TimeSpan timeSpan = DateTime.UtcNow - _playerData.energyRecovery.endTime;
				if (timeSpan.TotalSeconds >= 0)
				{
					// compensation offline recovery...
					int count = (int)(timeSpan.TotalSeconds / levelData.energy.recoveryCooldown);
					_playerData.stats.energy += levelData.energy.recoveryReward.count * count;

					SetEnergyRecovery(DateTime.UtcNow.AddSeconds(levelData.energy.recoveryCooldown));

					AddStats(new RewardData[1]
					{
						levelData.energy.recoveryReward
					}, true);
				}
			}
			else
			{
				_isEnergyRecovery = false;
			}
		}
	}


	public void SetMatch3Cooldown()
	{
		if (!_isMatch3Cooldown)
		{
			_playerData.match3Cooldown.endTime = DateTime.UtcNow.AddSeconds(GetCurrentLevelData().match3Config.cooldown);

			_isMatch3Cooldown = true;

			_gameplayEvents.Match3ReadyToPlay?.Invoke(false);
		}
	}
	private void Match3CooldownUpdate()
	{
		if (_isMatch3Cooldown)
		{
			TimeSpan timeSpan = DateTime.UtcNow - _playerData.match3Cooldown.endTime;
			if (timeSpan.TotalSeconds >= 0)
			{
				_isMatch3Cooldown = false;

				_gameplayEvents.Match3ReadyToPlay?.Invoke(true);
			}
		}
	}





	public bool AddStats(RewardData[] rewards, bool isSend = false, Action<RewardData[]> callback = null)
	{
		foreach (RewardData rewardData in rewards)
		{
			if (!_playerStats[rewardData.item.type](rewardData.count, true, false))
			{
				return false;
			}
		}

		callback?.Invoke(rewards);

		if (isSend)
		{
			_gameplayEvents.StatsAddReward?.Invoke(rewards);
		}

		return true;
	}
	public bool TakeStats(RewardData[] rewards, bool isSend = false, Action<RewardData[]> callback = null)
	{
		if (!HasStats(rewards, false, isSend))
		{
			return false;
		}

		foreach (RewardData rewardData in rewards)
		{
			if (!_playerStats[rewardData.item.type](rewardData.count, false, false))
			{
				return false;
			}
		}

		callback?.Invoke(rewards);

		if (isSend)
		{
			_gameplayEvents.StatsTakeReward?.Invoke(rewards);
		}

		return true;
	}
	public bool HasStats(RewardData[] rewards, bool add, bool isSend = false)
	{
		foreach (RewardData rewardData in rewards)
		{
			if (!_playerStats[rewardData.item.type](rewardData.count, add, true))
			{
				if (isSend)
				{
					_gameplayEvents.StatsFailReward?.Invoke(rewardData.item.type);
				}

				return false;
			}
		}

		return true;
	}



	public bool ShopPurchase(int shopItemIndex, Action<RewardData[]> callback = null)
	{
		try
		{
			ShopConfigData.ShopItem shopItem = GetCurrentLevelData().shopConfig.shopItems[shopItemIndex];

			if (!HasStats(shopItem.price, false) || !HasStats(shopItem.reward, true))
			{
				return false;
			}

			if (!TakeStats(shopItem.price, true))
			{
				return false;
			}

			if (!AddStats(shopItem.reward, false))
			{
				return false;
			}

			callback?.Invoke(shopItem.reward);
		}
		catch
		{
			Debug.Log($"<color=red>no index: {shopItemIndex} in shop.</color>");
			return false;
		}

		return true;
	}



	private bool StatsEnergy(int count, bool add, bool isCheck = false)
	{
		if (add)
		{
			if (isCheck)
			{
				return _playerData.stats.energy < GetCurrentLevelData().energy.max;
			}
			else
			{
				_playerData.stats.energy = Mathf.Min(_playerData.stats.energy + count, GetCurrentLevelData().energy.max);
			}
		}
		else
		{
			if (isCheck)
			{
				return _playerData.stats.energy >= count;
			}
			else
			{
				_playerData.stats.energy = Mathf.Max(_playerData.stats.energy - count, 0);
				
				SetEnergyRecovery();
			}
		}

		return true;
	}
	private bool StatsPoints(int count, bool add, bool isCheck = false)
	{
		if (add)
		{
			if (isCheck)
			{
				return _playerData.progress.points < GetCurrentLevelData().points;
			}
			else
			{
				_playerData.progress.points = Mathf.Min(_playerData.progress.points + count, GetCurrentLevelData().points);

				if (!StatsPoints(0, true, true))
				{
					LevelUp();
				}
			}
		}
		else
		{
			if (isCheck)
			{
				return _playerData.progress.points >= count;
			}
			else
			{
				_playerData.progress.points = Mathf.Max(_playerData.progress.points - count, 0);
			}
		}

		return true;
	}
	private bool StatsSpareParts(int count, bool add, bool isCheck = false)
	{
		if (add)
		{
			if (isCheck)
			{
				return true;
			}
			else
			{
				_playerData.stats.spareParts += count;
			}
		}
		else
		{
			if (isCheck)
			{
				return _playerData.stats.spareParts >= count;
			}
			else
			{
				_playerData.stats.spareParts = Mathf.Max(_playerData.stats.spareParts - count, 0);
			}
		}

		return true;
	}
	private bool StatsOil(int count, bool add, bool isCheck = false)
	{
		if (add)
		{
			if (isCheck)
			{
				return true;
			}
			else
			{
				_playerData.stats.oil += count;
			}
		}
		else
		{
			if (isCheck)
			{
				return _playerData.stats.oil >= count;
			}
			else
			{
				_playerData.stats.oil = Mathf.Max(_playerData.stats.oil - count, 0);
			}
		}

		return true;
	}
	private bool StatsCoins(int count, bool add, bool isCheck = false)
	{
		if (add)
		{
			if (isCheck)
			{
				return true;
			}
			else
			{
				_playerData.stats.coins += count;
			}
		}
		else
		{
			if (isCheck)
			{
				return _playerData.stats.coins >= count;
			}
			else
			{
				_playerData.stats.coins = Mathf.Max(_playerData.stats.coins - count, 0);
			}
		}

		return true;
	}


	public void LevelUp()
	{
		if (!_isLevelUp)
		{
			_isLevelUp = true;

			_gameplayEvents.StatsLevelUp?.Invoke(_playerData.progress.level);
		}
	}
	public void SetNewLevel(int value)
	{
		AddStats(GetCurrentLevelData().levelUpReward);

		_playerData.progress.level = value;
		_playerData.progress.points = 0;

		_playerData.energyRecovery.endTime = default;
		_playerData.match3Cooldown.endTime = default;

		GameConfigData.Level levelData = GetCurrentLevelData(true);

		_playerData.stats.energy = levelData.energy.max;

		_isLevelUp = false;

		_gameplayEvents.StatsUpdate?.Invoke();
		_gameplayEvents.GameRestart?.Invoke();
	}

	

	public GameConfigData.Level GetCurrentLevelData(bool forced = false)
	{
		if (_isCurrentLevelDataDirty || forced)
		{
			_currentLevelData = default;

			for (int i = 0; i < _gameConfig.levels.Length; i++)
			{
				_currentLevelData = _gameConfig.levels[i];
				if (_playerData.progress.level >= _currentLevelData.start && _playerData.progress.level <= _currentLevelData.end)
				{
					break;
				}
			}

			_isCurrentLevelDataDirty = false;
		}

		return _currentLevelData;
	}
}