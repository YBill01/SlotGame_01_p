using SlotGame.Profile;
using System;
using System.Collections.Generic;
using System.Linq;

public class SlotMachine : IDisposable
{
	private PlayerData _playerData;
	private SlotConfigData _config;
	private StatsBehaviour _stats;
	private GameplayEventsService _gameplayEvents;

	private SlotReel[] _reels;

	public struct ItemInfo
	{
		public int index;
		public int position;
	}
	public struct ReelInfo
	{
		public int index;
		public int position;
		public List<ItemInfo> itemsInfo;
	}

	public SlotMachine(GameplayEventsService gameplayEvents)
	{
		_playerData = Profile.Instance.Get<PlayerData>().data;
		
		_stats = App.Instance.Gameplay.Stats;
		_gameplayEvents = gameplayEvents;
	}

	public void Start(SlotConfigData config)
	{
		_config = config;

		_reels = new SlotReel[_config.pattern.numReels];

		FillReels();
		SendStart();
	}

	private void FillReels()
	{
		List<ItemInfo> itemsInfo = Shuffle();

		for (int i = 0; i < _config.pattern.numReels; i++)
		{
			_reels[i] = new SlotReel(_config.reel, itemsInfo);
			_reels[i].SetPosition(GetNewPosition());
		}
	}

	private List<ItemInfo> Shuffle()
	{
		List<ItemInfo> result = new List<ItemInfo>();

		// no shuffle...
		for (int i = 0; i < _config.reel.items.Length; i++)
		{
			result.Add(new ItemInfo
			{
				index = _config.reel.items[i].index,
				position = i
			});
		}

		return result;
	}

	private int GetNewPosition()
	{
		return UnityEngine.Random.Range(0, _config.reel.items.Length);
	}

	private void SendStart()
	{
		List<ReelInfo> result = new List<ReelInfo>();

		for (int i = 0; i < _config.pattern.numReels; i++)
		{
			result.Add(new ReelInfo
			{
				index = i,
				position = _reels[i].Position,
				itemsInfo = _reels[i].Items.Select((x, p) => new ItemInfo
				{
					index = x.index,
					position = p
				}).ToList()
			});
		}

		_gameplayEvents.SlotFillingReels?.Invoke(result);
	}

	public void Spin()
	{
		RewardData[] price = new RewardData[_config.spinPrice.Length];

		for (int i = 0; i < _config.spinPrice.Length; i++)
		{
			if (_config.spinPrice[i].item.type == ItemType.SpareParts)
			{
				price[i] = new RewardData
				{
					item = _config.spinPrice[i].item,
					count = _config.spinPrice[i].count * _playerData.slotProperties.sparePartsMultiplier
				};
			}
			else
			{
				price[i] = _config.spinPrice[i];
			}
		}

		if (_playerData.slotProperties.useOilBelay)
		{
			if (!_stats.HasStats(price, false, true) || !_stats.HasStats(_config.oilBelay.price, false, true) || !_stats.TakeStats(_config.oilBelay.price, true))
			{
				return;
			}
		}

		if (!_stats.TakeStats(price, true))
		{
			return;
		}

		List<ReelInfo> result = new List<ReelInfo>();

		for (int i = 0; i < _reels.Length; i++)
		{
			_reels[i].SetPosition(GetNewPosition());
			result.Add(new ReelInfo
			{
				index = i,
				position = _reels[i].Position
			});
		}

		_gameplayEvents.SlotSpin?.Invoke(result);
	}

	public void CheckSpinResult()
	{
		List<SlotPatternData.RewardTable> rewardsTableResult = new List<SlotPatternData.RewardTable>();
		List<List<int>> reelIndciesResult = new List<List<int>>();
		List<List<ItemInfo>> itemsInfoResult = new List<List<ItemInfo>>();

		bool isMatch = false;

		SlotPatternData pattern = _config.pattern;
		Array2D<bool> playGrid = _config.pattern.playGrid;

		Array2D<ItemInfo> dropGrid = new Array2D<ItemInfo>(_config.pattern.numReels, _config.pattern.numLines);
		for (int x = 0; x < pattern.numReels; x++)
		{
			SlotReel reel = _reels[x];
			for (int y = 0; y < pattern.numLines; y++)
			{
				int position = (reel.Position + y) % _config.reel.items.Length;
				dropGrid[x, y] = new ItemInfo
				{
					index = reel.Items[position].index,
					position = position
				};
			}
		}

		foreach (SlotPatternData.RewardTable rewardTable in _config.pattern.rewardTables)
		{
			if (TryMatch(dropGrid, rewardTable, out List<int> reelIndcies, out List<ItemInfo> itemsInfo))
			{
				rewardsTableResult.Add(rewardTable);
				reelIndciesResult.Add(reelIndcies);
				itemsInfoResult.Add(itemsInfo);

				isMatch = true;
			}
		}

		bool TryMatch(in Array2D<ItemInfo> dropGrid, in SlotPatternData.RewardTable rewardTable, out List<int> reelIndcies, out List<ItemInfo> itemsInfo)
		{
			reelIndcies = new List<int>();
			itemsInfo = new List<ItemInfo>();

			int index = -1;
			for (int x = 0; x < pattern.numReels; x++)
			{
				for (int y = 0; y < pattern.numLines; y++)
				{
					if (playGrid[x, y] && rewardTable.grid[x, y])
					{
						if (dropGrid[x, y].index == index || index == -1)
						{
							reelIndcies.Add(x);
							itemsInfo.Add(dropGrid[x, y]);
							index = dropGrid[x, y].index;
						}
						else
						{
							return false;
						}
					}
				}
			}

			return true;
		}

		if (isMatch)
		{
			RewardCalculate(rewardsTableResult, reelIndciesResult, itemsInfoResult);
		}
		else
		{
			FailCalculate();
		}
	}

	private void RewardCalculate(List<SlotPatternData.RewardTable> rewardsTable, List<List<int>> reelIndcies, List<List<ItemInfo>> itemsInfo)
	{
		List<List<RewardData[]>> rewardsResult = new List<List<RewardData[]>>();

		for (int i = 0; i < rewardsTable.Count; i++)
		{
			List<RewardData[]> rewardsLine = new List<RewardData[]>();

			for (int j = 0; j < itemsInfo[i].Count; j++)
			{
				RewardData[] rewardsItem = _config.reel.items[itemsInfo[i][j].position].reward;
				RewardData[] rewardsItemNew = new RewardData[rewardsItem.Length];

				for (int k = 0; k < rewardsItem.Length; k++)
				{
					rewardsItemNew[k] = new RewardData
					{
						item = rewardsItem[k].item,
						count = (int)(rewardsItem[k].count * _config.rewardMultiplier * rewardsTable[i].multiplier * _playerData.slotProperties.sparePartsMultiplier)
					};
				}

				rewardsLine.Add(rewardsItemNew);

				_stats.AddStats(rewardsItemNew);
			}

			rewardsResult.Add(rewardsLine);
		}

		_gameplayEvents.SlotSpinGoal?.Invoke(reelIndcies, itemsInfo, rewardsResult);
	}
	private void FailCalculate()
	{
		RewardData[] rewards = new RewardData[0];

		if (_playerData.slotProperties.useOilBelay)
		{
			float percent = UnityEngine.Random.Range(_config.oilBelay.rewardMinPercent, _config.oilBelay.rewardMaxPercent);

			ItemData itemData = null;
			int rewardAmount = 0;

			for (int i = 0; i < _config.spinPrice.Length; i++)
			{
				if (_config.spinPrice[i].item.type == ItemType.SpareParts)
				{
					itemData = _config.spinPrice[i].item;
					rewardAmount += (int)(_config.spinPrice[i].count * _playerData.slotProperties.sparePartsMultiplier * percent);
				}
			}

			rewards = new RewardData[1];
			rewards[0] = new RewardData
			{
				item = itemData,
				count = rewardAmount
			};

			_stats.AddStats(rewards);
		}

		_gameplayEvents.SlotSpinFail?.Invoke(rewards);
	}

	public void Dispose()
	{
		_reels = null;
	}
}