using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIMatch3Game : MonoBehaviour
{
	public Action<RewardData[], IUIStatsDrop> onRewardDrop;


	[SerializeField]
	private Vector2 m_slotSize;
	[SerializeField]
	private Vector2 m_slotSpacing;

	[Space]
	[SerializeField]
	private UIMatch3Item m_prefabItem;


	private Match3ConfigData _config;
	private GameplayEventsService _gameplayEvents;
	private UIEventsService _uiEvents;


	private Slot[,] _grid;

	
	protected CanvasGroup _canvasGroup;


	private void Awake()
	{
		_canvasGroup = GetComponent<CanvasGroup>();
	}

	private void OnEnable()
	{
		_gameplayEvents.Match3FillingGrid += FillingGrid;
		_gameplayEvents.Match3FillingItems += FillingItems;
		_gameplayEvents.Match3SwapItems += SwapItems;
		_gameplayEvents.Match3DisappearItems += DisappearItems;
		_gameplayEvents.Match3FallingItems += FallingItems;
	}
	private void OnDisable()
	{
		_gameplayEvents.Match3FillingGrid -= FillingGrid;
		_gameplayEvents.Match3FillingItems -= FillingItems;
		_gameplayEvents.Match3SwapItems -= SwapItems;
		_gameplayEvents.Match3DisappearItems -= DisappearItems;
		_gameplayEvents.Match3FallingItems -= FallingItems;
	}

	public void Init(Match3ConfigData config, GameplayEventsService gameplayEvents, UIEventsService uiEvents)
	{
		_config = config;
		_gameplayEvents = gameplayEvents;
		_uiEvents = uiEvents;

		CreateSlots();
	}

	public void StartGame()
	{
		ClearAllItems();

		_uiEvents.Match3Start?.Invoke();
	}
	public void EndGame()
	{
		ClearAllItems();
	}

	private void CreateSlots()
	{
		_grid = new Slot[_config.size.x, _config.size.y];

		for (int y = 0; y < _config.size.y; y++)
		{
			for (int x = 0; x < _config.size.x; x++)
			{
				Slot slot = new Slot(new Vector2Int(x, y))
				{
					index = -1,
				};

				_grid[x, y] = slot;
			}
		}
	}

	

	private void CreateItems(List<Match3.ItemInfo> itemsInfo)
	{
		foreach (Match3.ItemInfo itemInfo in itemsInfo)
		{
			Slot slot = _grid[itemInfo.coords.x, itemInfo.coords.y];
			slot.index = itemInfo.index;

			UIMatch3Item item = Instantiate(m_prefabItem, transform);

			item.SetData(_config.items[slot.index].item);
			item.SetCoords(slot.coords);
			item.SetPosition(
				slot.coords.x * m_slotSize.x + slot.coords.x * m_slotSpacing.x,
				-slot.coords.y * m_slotSize.y - slot.coords.y * m_slotSpacing.y
			);

			item.Init(MoveItem);
			item.Appear();

			slot.item = item;
		}
	}
	private void ClearItem(int coordX, int coordY)
	{
		Slot slot = _grid[coordX, coordY];

		if (slot.item != null)
		{
			Destroy(slot.item.gameObject);
		}

		slot.index = -1;
		slot.item = null;
	}
	private void ClearItem(Match3.ItemInfo itemInfo)
	{
		ClearItem(itemInfo.coords.x, itemInfo.coords.y);
	}
	private void ClearAllItems()
	{
		for (int y = 0; y < _config.size.y; y++)
		{
			for (int x = 0; x < _config.size.x; x++)
			{
				ClearItem(x, y);
			}
		}
	}




	private void FillingGrid(List<Match3.ItemInfo> itemsInfo)
	{
		//ClearAllItems();
		CreateItems(itemsInfo);
	}
	private void FillingItems(List<Match3.ItemInfo> itemsInfo)
	{
		CreateItems(itemsInfo);

		AnimatedDelay(0.25f, () =>
		{
			_uiEvents.Match3FindMatches?.Invoke();
		}).Forget();
	}

	private void SwapItems(Match3.ItemInfo itemInfo1, Match3.ItemInfo itemInfo2)
	{
		Slot slot1 = _grid[itemInfo1.coords.x, itemInfo1.coords.y];
		Slot slot2 = _grid[itemInfo2.coords.x, itemInfo2.coords.y];

		int index1 = slot1.index;
		int index2 = slot2.index;

		UIMatch3Item item1 = slot1.item;
		UIMatch3Item item2 = slot2.item;

		slot1.index = index2;
		slot1.item = item2;
		slot1.item.SetCoords(slot1.coords);
		slot1.item.DoPosition(
			slot1.coords.x * m_slotSize.x + slot1.coords.x * m_slotSpacing.x,
			-slot1.coords.y * m_slotSize.y - slot1.coords.y * m_slotSpacing.y
		);

		slot2.index = index1;
		slot2.item = item1;
		slot2.item.SetCoords(slot2.coords);
		slot2.item.DoJumpPosition(
			slot2.coords.x * m_slotSize.x + slot2.coords.x * m_slotSpacing.x,
			-slot2.coords.y * m_slotSize.y - slot2.coords.y * m_slotSpacing.y
		);

		AnimatedDelay(0.25f, () =>
		{
			_uiEvents.Match3FindMatches?.Invoke();
		}).Forget();
	}

	private void DisappearItems(List<Match3.ItemInfo> itemsInfo, List<RewardData[]> rewardsList)
	{
		for (int i = 0; i < itemsInfo.Count; i++)
		{
			DisappearItem(itemsInfo[i], rewardsList[i]);
		}

		App.Instance.Services
			.Get<UIService>()
			.Get<UISoundService>()
			.PlaySFXOnceShot(10);

		AnimatedDelay(0.25f, () =>
		{
			_uiEvents.Match3FallingItems?.Invoke();
		}).Forget();
	}
	private void DisappearItem(Match3.ItemInfo itemInfo, RewardData[] rewards)
	{
		Slot slot = _grid[itemInfo.coords.x, itemInfo.coords.y];
		UIMatch3Item item = slot.item;

		item.Disappear(() =>
		{
			ClearItem(itemInfo);
		});

		onRewardDrop?.Invoke(rewards, item);
	}

	private void FallingItems(List<Match3.ItemInfo> itemsInfoFrom, List<Match3.ItemInfo> itemsInfoTo)
	{
		for (int i = 0; i < itemsInfoFrom.Count; i++)
		{
			Slot slot1 = _grid[itemsInfoFrom[i].coords.x, itemsInfoFrom[i].coords.y];
			Slot slot2 = _grid[itemsInfoTo[i].coords.x, itemsInfoTo[i].coords.y];

			slot2.index = slot1.index;
			slot2.item = slot1.item;
			slot1.index = -1;
			slot1.item = null;

			slot2.item.SetCoords(slot2.coords);
			slot2.item.DoFallPosition(
				slot2.coords.x * m_slotSize.x + slot2.coords.x * m_slotSpacing.x,
				-slot2.coords.y * m_slotSize.y - slot2.coords.y * m_slotSpacing.y
			);
		}

		AnimatedDelay(0.25f, () =>
		{
			_uiEvents.Match3FillingItems?.Invoke();
		}).Forget();
	}

	private void MoveItem(Vector2Int coords, Vector2Int axisDirection)
	{
		Match3.ItemInfo itemInfo = new Match3.ItemInfo
		{
			index = _grid[coords.x, coords.y].index,
			coords = coords
		};

		if (_uiEvents.TryMatch3MoveItem == null || !_uiEvents.TryMatch3MoveItem.Invoke(itemInfo, axisDirection))
		{
			WrongMoveItem(itemInfo);
		}
	}
	private void WrongMoveItem(Match3.ItemInfo itemInfo)
	{
		_grid[itemInfo.coords.x, itemInfo.coords.y].item.WrongMove();
	}





	private async UniTaskVoid AnimatedDelay(float duration, Action action)
	{
		Animated(true);
		await UniTask.WaitForSeconds(duration, cancellationToken: destroyCancellationToken);
		Animated(false);

		action?.Invoke();
	}
	private void Animated(bool value)
	{
		_canvasGroup.blocksRaycasts = !value;
	}

	private class Slot
	{
		public int index;
		public readonly Vector2Int coords;
		public UIMatch3Item item;

		public Slot(Vector2Int coords)
		{
			this.coords = coords;
		}
	}
}