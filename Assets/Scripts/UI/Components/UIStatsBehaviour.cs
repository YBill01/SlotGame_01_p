using Cysharp.Threading.Tasks;
using SlotGame.Profile;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIStatsBehaviour : MonoBehaviour
{
	public Action onShopOpen;

	[Space]
	[SerializeField]
	private UISmoothCollectingEffect m_smoothCollecting;

	[Space]
	[SerializeField]
	private UIEnergyPanel m_energyPanel;
	[SerializeField]
	private UILevelProgressPanel m_levelProgressPanel;
	[SerializeField]
	private UIItemPanel m_sparePartsPanel;
	[SerializeField]
	private UIItemPanel m_oilPanel;
	[SerializeField]
	private UIItemPanel m_coinsPanel;

	private PlayerData _playerData;
	private StatsBehaviour _stats;
	private GameplayEventsService _gameplayEvents;
	private UIEventsService _uiEvenetsService;

	private Dictionary<ItemType, IUIStats> _statsPanels;

	private void Awake()
	{
		_playerData = Profile.Instance.Get<PlayerData>().data;
		_stats = App.Instance.Gameplay.Stats;
		_gameplayEvents = App.Instance.Services.Get<GameplayEventsService>();
		_uiEvenetsService = App.Instance.Services.Get<UIEventsService>();

		_statsPanels = new Dictionary<ItemType, IUIStats>
		{
			{ ItemType.Energy, m_energyPanel },
			{ ItemType.Points, m_levelProgressPanel },
			{ ItemType.SpareParts, m_sparePartsPanel },
			{ ItemType.Oil, m_oilPanel },
			{ ItemType.Coins, m_coinsPanel }
		};
	}

	private async UniTaskVoid Start()
	{
		await UniTask.NextFrame();

		SetStats();
	}

	private void OnEnable()
	{
		_gameplayEvents.StatsAddReward += StatsAddReward;
		_gameplayEvents.StatsTakeReward += StatsTakeReward;
		_gameplayEvents.StatsFailReward += StatsFailReward;
		_gameplayEvents.StatsUpdate += StatsUpdate;

		_uiEvenetsService.ShopOpen += ShopOpen;
	}
	private void OnDisable()
	{
		_gameplayEvents.StatsAddReward -= StatsAddReward;
		_gameplayEvents.StatsTakeReward -= StatsTakeReward;
		_gameplayEvents.StatsFailReward -= StatsFailReward;
		_gameplayEvents.StatsUpdate -= StatsUpdate;

		_uiEvenetsService.ShopOpen -= ShopOpen;
	}

	private void Update()
	{
		if (_stats.IsEnergyRecovery)
		{
			float value = 1.0f - (float)((_playerData.energyRecovery.endTime - DateTime.UtcNow).TotalSeconds / _stats.GetCurrentLevelData().energy.recoveryCooldown);
			m_energyPanel?.SetRecoveryProgress(value);
		}
	}

	private void SetStats()
	{
		//m_energyPanel?.SetAmount(_playerData.stats.energy);
		//m_energyPanel?.SetRecoveryProgress(0.0f);

		//m_levelProgressPanel?.SetLevel(_playerData.progress.level);
		//m_levelProgressPanel?.SetAmount(_playerData.progress.points);

		m_sparePartsPanel?.SetAmount(_playerData.stats.spareParts);
		m_oilPanel?.SetAmount(_playerData.stats.oil);
		m_coinsPanel?.SetAmount(_playerData.stats.coins);
	}

	private void StatsUpdate()
	{
		m_energyPanel?.SetAmount(_playerData.stats.energy);
		m_energyPanel?.SetRecoveryProgress(0.0f);

		m_levelProgressPanel?.SetLevel(_playerData.progress.level);
		m_levelProgressPanel?.SetAmount(_playerData.progress.points);

		m_sparePartsPanel?.SetAmount(_playerData.stats.spareParts);
		m_oilPanel?.SetAmount(_playerData.stats.oil);
		m_coinsPanel?.SetAmount(_playerData.stats.coins);
	}

	public void AddReward(RewardData[] reward, RectTransform inRectTransform)
	{
		Vector3 inPosition = transform.InverseTransformPoint(inRectTransform.position);
		Rect inRect = new Rect(new Vector2(inPosition.x + inRectTransform.rect.position.x, inPosition.y + inRectTransform.rect.position.y), inRectTransform.rect.size);

		UISmoothCollectingEffect.StackInfo[] stacks = new UISmoothCollectingEffect.StackInfo[reward.Length];
		for (int i = 0; i < reward.Length; i++)
		{
			RectTransform outRectTransform = _statsPanels[reward[i].item.type].GetRectTransformCollecting();
			Vector3 outPosition = transform.InverseTransformPoint(outRectTransform.position);
			Rect outRect = new Rect(new Vector2(outPosition.x + outRectTransform.rect.position.x, outPosition.y + outRectTransform.rect.position.y), outRectTransform.rect.size);

			UISmoothCollectingEffect.ItemInfo[] items = new UISmoothCollectingEffect.ItemInfo[1];
			for (int j = 0; j < items.Length; j++)
			{
				items[j] = new UISmoothCollectingEffect.ItemInfo
				{
					icon = reward[i].item.view.icon,
					amount = reward[i].count,
					action = _statsPanels[reward[i].item.type].Add
				};
			}

			stacks[i] = new UISmoothCollectingEffect.StackInfo
			{
				items = items,
				startRect = inRect,
				endRect = outRect,
			};
		}

		m_smoothCollecting.Show(stacks);
	}

	private void StatsAddReward(RewardData[] rewards)
	{
		foreach (RewardData reward in rewards)
		{
			_statsPanels[reward.item.type].Add(reward.count);
		}
	}
	private void StatsTakeReward(RewardData[] rewards)
	{
		foreach (RewardData reward in rewards)
		{
			_statsPanels[reward.item.type].Take(reward.count);
		}
	}
	
	private void StatsFailReward(ItemType itemType)
	{
		if (itemType != ItemType.Coins && !_stats.IsMatch3Cooldown)
		{
			_gameplayEvents.GameMatch3?.Invoke();
		}
		else
		{
			ShopOpen();
		}
	}

	private void ShopOpen()
	{
		onShopOpen?.Invoke();
	}


}