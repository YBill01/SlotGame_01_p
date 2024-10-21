using System;
using UnityEngine;

public class UIEventsService : IService
{
	public Action GoToPlay;
	public Action BackToHome;
	public Action Exit;

	public Action<bool> SoundOn;
	public Action<bool> MusicOn;

	//public Action<> AddReward;
	//public Action<> TakeReward;
	
	public Action ShopOpen;
	public Action<int> ShopPurchase;


	public Action Match3Start;
	public delegate bool ActionTryMatch3MoveItem(Match3.ItemInfo itemInfo, Vector2Int axisDirection);
	public ActionTryMatch3MoveItem TryMatch3MoveItem;
	public Action Match3FindMatches;
	public Action Match3FallingItems;
	public Action Match3FillingItems;

	public Action SlotStart;
	public Action SlotSpin;
	public Action SlotEndSpin;

	public Action<bool> SlotAnimation;
}