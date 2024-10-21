using System;
using System.Collections.Generic;

public class GameplayEventsService : IService
{
	public Action<Gameplay> Initialized;

	public Action Home;
	public Action Game;

	public Action GameSlot;
	public Action GameMatch3;

	public Action StatsUpdate;
	public Action<int> StatsLevelUp;
	public Action<RewardData[]> StatsAddReward;
	public Action<RewardData[]> StatsTakeReward;
	public Action<ItemType> StatsFailReward;

	public Action GameRestart;

	public Action<bool> Match3ReadyToPlay;

	public Action<List<Match3.ItemInfo>> Match3FillingGrid;
	public Action<List<Match3.ItemInfo>> Match3FillingItems;
	public Action<Match3.ItemInfo, Match3.ItemInfo> Match3SwapItems;
	public Action<List<Match3.ItemInfo>, List<RewardData[]>> Match3DisappearItems;
	public Action<List<Match3.ItemInfo>, List<Match3.ItemInfo>> Match3FallingItems;

	public Action<List<SlotMachine.ReelInfo>> SlotFillingReels;
	public Action<List<SlotMachine.ReelInfo>> SlotSpin;
	public Action<List<List<int>>, List<List<SlotMachine.ItemInfo>>, List<List<RewardData[]>>> SlotSpinGoal;
	public Action<RewardData[]> SlotSpinFail;


}