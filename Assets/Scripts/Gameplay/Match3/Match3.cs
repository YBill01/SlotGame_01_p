using System;
using System.Collections.Generic;
using UnityEngine;

public class Match3 : IDisposable
{
	private Match3ConfigData _config;
	private StatsBehaviour _stats;
	private GameplayEventsService _gameplayEvents;

	private Match3Item[,] _grid;

	public struct ItemInfo
	{
		public int index;
		public Vector2Int coords;
	}

	public Match3(GameplayEventsService gameplayEvents)
	{
		_stats = App.Instance.Gameplay.Stats;

		_gameplayEvents = gameplayEvents;
	}
	
	public void Start(Match3ConfigData config)
	{
		_config = config;

		_stats.SetMatch3Cooldown();

		_grid = new Match3Item[_config.size.x, _config.size.y];

		FillGrid();
		while (FindMatches(out List<ItemInfo> matchesItemsInfo) || !FindMove(out ItemInfo moveItemInfo, out Vector2Int axisDirection))
		{
			//Debug.Log(itemsInfo.Count);

			FillGrid();
		}

		SendStart();
	}

	private void FillGrid()
	{
		for (int y = 0; y < _config.size.y; y++)
		{
			for (int x = 0; x < _config.size.x; x++)
			{
				_grid[x, y] = GetNewItem();
			}
		}
	}
	private void FillGrid(List<ItemInfo> itemsInfo, out List<ItemInfo> newItemsInfo)
	{
		newItemsInfo = new List<ItemInfo>();

		foreach (ItemInfo itemInfo in itemsInfo)
		{
			_grid[itemInfo.coords.x, itemInfo.coords.y] = GetNewItem();
			newItemsInfo.Add(new ItemInfo
			{
				index = _grid[itemInfo.coords.x, itemInfo.coords.y].index,
				coords = itemInfo.coords
			});
		}
	}

	private void ClearItem(ItemInfo itemInfo)
	{
		_grid[itemInfo.coords.x, itemInfo.coords.y] = null;
	}

	private Match3Item GetNewItem()
	{
		float randomValue = UnityEngine.Random.Range(0.0f, 1.0f);

		float allProbability = 0.0f;
		foreach (Match3ConfigData.Item item in _config.items)
		{
			allProbability += item.probability;
		}
		
		int index = 0;
		float cumulative = 0.0f;
		for (int i = 0; i < _config.items.Length; i++)
		{
			cumulative += _config.items[i].probability / allProbability;
			if (randomValue <= cumulative)
			{
				index = i;
				break;
			}
		}

		Match3Item result = new Match3Item(_config.items[index].item)
		{
			index = index
		};

		return result;
	}
	private bool TryGetItem(in ItemInfo itemInfo, out Match3Item item)
	{
		item = null;

		if (HasItem(itemInfo.coords.x, itemInfo.coords.y))
		{
			item = _grid[itemInfo.coords.x, itemInfo.coords.y];
		}

		return item is not null;
	}
	private Match3Item GetItem(in ItemInfo itemInfo)
	{
		return _grid[itemInfo.coords.x, itemInfo.coords.y];
	}
	private ItemInfo GetItemInfo(int coordX, int coordY)
	{
		return new ItemInfo
		{
			index = _grid[coordX, coordY].index,
			coords = new Vector2Int(coordX, coordY)
		};
	}

	private void SetItem(int coordX, int coordY, Match3Item item)
	{
		_grid[coordX, coordY] = item;
	}

	private bool HasItem(int coordX, int coordY)
	{
		if (coordX >= 0 && coordX < _config.size.x &&
			coordY >= 0 && coordY < _config.size.y)
		{
			return _grid[coordX, coordY] is not null;
		}

		return false;
	}

	private void SendStart()
	{
		List<ItemInfo> result = new List<ItemInfo>();

		for (int y = 0; y < _config.size.y; y++)
		{
			for (int x = 0; x < _config.size.x; x++)
			{
				result.Add(GetItemInfo(x, y));
			}
		}

		_gameplayEvents.Match3FillingGrid?.Invoke(result);
	}

	public bool TryMoveItem(ItemInfo itemInfo, Vector2Int axisDirection)
	{
		if (CheckMoveItem(itemInfo, axisDirection, out ItemInfo itemInfo2))
		{
			SwapItems(itemInfo, itemInfo2);

			_gameplayEvents.Match3SwapItems?.Invoke(itemInfo, itemInfo2);

			return true;
		}

		return false;
	}

	public bool CheckMoveItem(ItemInfo itemInfo, Vector2Int axisDirection, out ItemInfo itemInfo2)
	{
		bool result = false;

		itemInfo2 = new ItemInfo
		{
			coords = itemInfo.coords + axisDirection
		};

		if (TryGetItem(itemInfo2, out Match3Item item))
		{
			SwapItems(itemInfo, itemInfo2);

			if (IsMatch(itemInfo) || IsMatch(itemInfo2))
			{
				result = true;
			}

			SwapItems(itemInfo, itemInfo2);
		}

		return result;
	}

	public void MatchesProcess()
	{
		if (FindMatches(out List<ItemInfo> itemsInfo))
		{
			DisappearItems(itemsInfo);
		}
	}

	public void FallingItemsProcess()
	{
		List<ItemInfo> itemsInfoFrom = new List<ItemInfo>();
		List<ItemInfo> itemsInfoTo = new List<ItemInfo>();

		bool send = false;

		for (int y = _config.size.y - 1; y >= 0; y--)
		{
			for (int x = _config.size.x - 1; x >= 0; x--)
			{
				if (HasItem(x, y))
				{
					ItemInfo itemInfoFrom = GetItemInfo(x, y);
					if (TryFallItem(itemInfoFrom, out ItemInfo itemInfoTo))
					{
						itemsInfoFrom.Add(itemInfoFrom);
						itemsInfoTo.Add(itemInfoTo);

						SetItem(itemInfoTo.coords.x, itemInfoTo.coords.y, GetItem(itemInfoFrom));
						SetItem(itemInfoFrom.coords.x, itemInfoFrom.coords.y, null);

						send = true;
					}
				}
			}
		}

		bool TryFallItem(in ItemInfo itemInfoFrom, out ItemInfo itemInfoTo)
		{
			itemInfoTo = default;

			int coordY = itemInfoFrom.coords.y;
			for (int i = itemInfoFrom.coords.y + 1; i < _config.size.y; i++)
			{
				if (!HasItem(itemInfoFrom.coords.x, i))
				{
					coordY = i;
				}
			}

			if (coordY > itemInfoFrom.coords.y)
			{
				itemInfoTo = new ItemInfo
				{
					index = itemInfoFrom.index,
					coords = new Vector2Int(itemInfoFrom.coords.x, coordY),
				};

				return true;
			}

			return false;
		}

		if (send)
		{
			_gameplayEvents.Match3FallingItems?.Invoke(itemsInfoFrom, itemsInfoTo);
		}
		else
		{
			FillingItems();
		}
	}

	public void FillingItems()
	{
		List<ItemInfo> itemsInfo = new List<ItemInfo>();

		for (int y = 0; y < _config.size.y; y++)
		{
			for (int x = 0; x < _config.size.x; x++)
			{
				if (!HasItem(x, y))
				{
					itemsInfo.Add(new ItemInfo
					{
						coords = new Vector2Int(x, y)
					});
				}
			}
		}

		List<ItemInfo> newItemsInfo;

		FillGrid(itemsInfo, out newItemsInfo);
		while (!FindMove(out ItemInfo moveItemInfo, out Vector2Int axisDirection))
		{
			FillGrid(itemsInfo, out newItemsInfo);
		}

		_gameplayEvents.Match3FillingItems?.Invoke(newItemsInfo);
	}

	private void SwapItems(ItemInfo itemInfo1, ItemInfo itemInfo2)
	{
		if (TryGetItem(itemInfo1, out Match3Item item1) && TryGetItem(itemInfo2, out Match3Item item2))
		{
			_grid[itemInfo1.coords.x, itemInfo1.coords.y] = item2;
			_grid[itemInfo2.coords.x, itemInfo2.coords.y] = item1;
		}
	}

	private bool IsMatch(ItemInfo itemInfo)
	{
		Match3Item inspectItem = GetItem(itemInfo);

		int hCount = 0;
		int vCount = 0;

		hCount = NearestCount(itemInfo, Vector2Int.left) + NearestCount(itemInfo, Vector2Int.right);

		if (hCount >= 2)
		{
			return true;
		}

		vCount = NearestCount(itemInfo, Vector2Int.up) + NearestCount(itemInfo, Vector2Int.down);

		if (vCount >= 2)
		{
			return true;
		}

		int NearestCount(in ItemInfo itemInfo, Vector2Int axisDirection)
		{
			int result = 0;

			Match3Item item;
			for (int x = 1; x < 3; x++)
			{
				if (TryGetItem(new ItemInfo { coords = itemInfo.coords + axisDirection * x }, out item))
				{
					if (item.index == inspectItem.index)
					{
						result++;
					}
					else
					{
						break;
					}
				}
				else
				{
					break;
				}
			}

			return result;
		}

		return false;
	}
	private bool FindMatches(out List<ItemInfo> matchesItemsInfo)
	{
		bool result = false;

		matchesItemsInfo = new List<ItemInfo>();

		for (int y = 0; y < _config.size.y; y++)
		{
			for (int x = 0; x < _config.size.x; x++)
			{
				ItemInfo itemInfo = GetItemInfo(x, y);
				if (IsMatch(itemInfo))
				{
					matchesItemsInfo.Add(itemInfo);

					result = true;
				}
			}
		}

		return result;
	}

	private bool FindMove(out ItemInfo findItemInfo, out Vector2Int axisDirection)
	{
		findItemInfo = default;
		axisDirection = default;

		for (int y = 0; y < _config.size.y; y++)
		{
			for (int x = 0; x < _config.size.x; x++)
			{
				ItemInfo itemInfo1 = GetItemInfo(x, y);
				ItemInfo itemInfo2;

				if (CheckMoveItem(itemInfo1, Vector2Int.right, out itemInfo2))
				{
					findItemInfo = itemInfo1;
					axisDirection = Vector2Int.right;

					return true;
				}
				else if (CheckMoveItem(itemInfo1, Vector2Int.up, out itemInfo2))
				{
					findItemInfo = itemInfo1;
					axisDirection = Vector2Int.up;

					return true;
				}
			}
		}

		return false;
	}

	private void DisappearItems(List<ItemInfo> itemsInfo)
	{
		foreach (ItemInfo itemInfo in itemsInfo)
		{
			ClearItem(itemInfo);
		}

		RewardCalculate(itemsInfo);
	}

	private void RewardCalculate(List<ItemInfo> itemsInfo)
	{
		List<RewardData[]> rewardsResult = new List<RewardData[]>();

		for (int i = 0; i < itemsInfo.Count; i++)
		{
			RewardData[] rewardsItem = _config.items[itemsInfo[i].index].item.reward;
			RewardData[] rewardsItemNew = new RewardData[rewardsItem.Length];

			for (int j = 0; j < rewardsItem.Length; j++)
			{
				rewardsItemNew[j] = new RewardData
				{
					item = rewardsItem[j].item,
					count = (int)(rewardsItem[j].count * _config.rewardMultiplier)
				};
			}

			rewardsResult.Add(rewardsItemNew);

			_stats.AddStats(rewardsItemNew);
		}

		_gameplayEvents.Match3DisappearItems?.Invoke(itemsInfo, rewardsResult);
	}

	public void Dispose()
	{
		_grid = null;
	}
}