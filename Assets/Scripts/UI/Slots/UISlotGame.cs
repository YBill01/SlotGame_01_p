using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UISlotGame : MonoBehaviour
{
	public Action<RewardData[], IUIStatsDrop> onRewardDrop;
	public Action<bool> onAnimation;

	[Space]
	[SerializeField]
	private UISlotReel[] m_reels;

	[SerializeField]
	private RectTransform m_content;

	[Space]
	[SerializeField]
	private UISlotTiles m_tiles;

	[SerializeField]
	private UISlotFX m_fx;

	[Space]
	[SerializeField]
	private UIStatsDropComponent m_drop;

	private SlotConfigData _config;
	private GameplayEventsService _gameplayEvents;
	private UIEventsService _uiEvents;

	protected CanvasGroup _canvasGroup;

	private void Awake()
	{
		_canvasGroup = GetComponent<CanvasGroup>();
	}

	private void OnEnable()
	{
		_gameplayEvents.SlotFillingReels += FillingReels;
		_gameplayEvents.SlotSpin += Spin;
		_gameplayEvents.SlotSpinGoal += SpinGoal;
		_gameplayEvents.SlotSpinFail += SpinFail;
	}
	private void OnDisable()
	{
		_gameplayEvents.SlotFillingReels -= FillingReels;
		_gameplayEvents.SlotSpin -= Spin;
		_gameplayEvents.SlotSpinGoal -= SpinGoal;
		_gameplayEvents.SlotSpinFail -= SpinFail;
	}

	public void Init(SlotConfigData config, GameplayEventsService gameplayEvents, UIEventsService uiEvents)
	{
		_config = config;
		_gameplayEvents = gameplayEvents;
		_uiEvents = uiEvents;

		m_tiles.SetTiles(_config.pattern);
	}

	public void StartGame()
	{
		ClearAllReels();

		_uiEvents.SlotStart?.Invoke();
	}
	public void EndGame()
	{
		ClearAllReels();
	}

	private void FillingReels(List<SlotMachine.ReelInfo> reelsInfo)
	{
		CreateReels(reelsInfo);
	}

	private void CreateReels(List<SlotMachine.ReelInfo> reelsInfo)
	{
		for (int i = 0; i < reelsInfo.Count; i++)
		{
			m_reels[i].SetData(_config.reel);
			m_reels[i].CreateItems(reelsInfo[i].itemsInfo);
			m_reels[i].SetPosition(reelsInfo[i].position);
		}
	}

	private void ClearAllReels()
	{
		foreach (UISlotReel reel in m_reels)
		{
			reel.ClearItems();
		}
	}

	private void Spin(List<SlotMachine.ReelInfo> reelsInfo)
	{
		foreach (SlotMachine.ReelInfo reelInfo in reelsInfo)
		{
			m_reels[reelInfo.index].DoPosition(reelInfo.position, _config.scrollTime, _config.scrollCount);
		}

		App.Instance.Services
				.Get<UIService>()
				.Get<UISoundService>()
				.PlaySFXOnceShot(12);

		App.Instance.Services
				.Get<UIService>()
				.Get<UISoundService>()
				.PlaySFX(13);

		m_content.DOShakeAnchorPos(_config.scrollTime, 12, 16, 90, true, false)
			//.SetEase(Ease.OutSine);
			.SetEase(Ease.Linear);

		m_fx.SparksEffect(true);
		AnimatedDelay(_config.scrollTime, () =>
		{
			m_fx.SparksEffect(false);
			_uiEvents.SlotEndSpin?.Invoke();

			App.Instance.Services
				.Get<UIService>()
				.Get<UISoundService>()
				.StopSFX();

			App.Instance.Services
				.Get<UIService>()
				.Get<UISoundService>()
				.PlaySFXOnceShot(14);
		}).Forget();
	}

	private void SpinGoal(List<List<int>> reelIndcies, List<List<SlotMachine.ItemInfo>> itemsInfo, List<List<RewardData[]>> rewardsList)
	{
		float delay = 0.2f;

		for (int i = 0; i < itemsInfo.Count; i++)
		{
			FlashLine(delay * i, reelIndcies[i], itemsInfo[i], rewardsList[i]).Forget();
		}

		AnimatedDelay((delay * (itemsInfo.Count - 1)) + 0.3f, () =>
		{
			// end flashing items...
		}).Forget();
	}
	private void SpinFail(RewardData[] rewards)
	{
		for (int i = 0; i < rewards.Length; i++)
		{
			onRewardDrop?.Invoke(rewards, m_drop);

			App.Instance.Services
				.Get<UIService>()
				.Get<UISoundService>()
				.PlaySFXOnceShot(10);
		}
	}

	private async UniTaskVoid FlashLine(float delay, List<int> reelIndcies, List<SlotMachine.ItemInfo> itemsInfo, List<RewardData[]> rewardsList)
	{
		await UniTask.WaitForSeconds(delay, cancellationToken: destroyCancellationToken);

		for (int i = 0; i < reelIndcies.Count; i++)
		{
			m_reels[reelIndcies[i]].DoFlashItem(itemsInfo[i]);

			onRewardDrop?.Invoke(rewardsList[i], m_reels[reelIndcies[i]].GetItem(itemsInfo[i]));
		}

		App.Instance.Services
			.Get<UIService>()
			.Get<UISoundService>()
			.PlaySFXOnceShot(10);
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

		onAnimation?.Invoke(value);
	}
}