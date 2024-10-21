using SlotGame.Profile;
using System;
using UnityEngine;

public class UISlotMachineScreen : UIGameScreen
{
	[Space]
	[SerializeField]
	private UISlotGame m_game;

	[Space]
	[SerializeField]
	private UISlotSpinPanel m_spinPanel;
	[SerializeField]
	private UISlotOilPanel m_oilPanel;
	[SerializeField]
	private UISlotSparePartsPanel m_sparePartsPanel;

	private PlayerData _playerData;
	private SlotConfigData _config;
	private GameplayEventsService _gameplayEvents;
	private UIEventsService _uiEvents;

	protected override void OnInit()
	{
		base.OnInit();

		_config = App.Instance.Gameplay.Stats.GetCurrentLevelData().slotConfig;
		_playerData = Profile.Instance.Get<PlayerData>().data;
		_gameplayEvents = App.Instance.Services.Get<GameplayEventsService>();
		_uiEvents = App.Instance.Services.Get<UIEventsService>();

		m_game.Init(_config, _gameplayEvents, _uiEvents);
	}

	protected override void OnPreShow()
	{
		GameRestart();
	}
	protected override void OnHide()
	{
		m_game.EndGame();
	}

	private void OnEnable()
	{
		_gameplayEvents.GameRestart += GameRestart;

		m_spinPanel.onSpin += OnSpin;
		m_game.onRewardDrop += RewardDrop;
		m_game.onAnimation += GameAnimation;
	}
	private void OnDisable()
	{
		_gameplayEvents.GameRestart -= GameRestart;

		m_spinPanel.onSpin -= OnSpin;
		m_game.onRewardDrop -= RewardDrop;
		m_game.onAnimation -= GameAnimation;
	}

	private void GameRestart()
	{
		_config = App.Instance.Gameplay.Stats.GetCurrentLevelData().slotConfig;

		m_spinPanel.SetData(_config);
		m_oilPanel.SetData(_config, _playerData);
		m_sparePartsPanel.SetData(_config, _playerData);

		m_game.Init(_config, _gameplayEvents, _uiEvents);

		m_game.StartGame();
	}

	/*public void Init()
	{
		if(SlotState != SlotStates.None)
		{
			return;
		}

		SlotState = SlotStates.Ready;



	}*/


	public void OnSpin()
	{
		App.Instance.Services.Get<UIEventsService>().SlotSpin?.Invoke();
	}

	/*public void SpinStop()
	{

	}*/

	private void RewardDrop(RewardData[] reward, IUIStatsDrop target)
	{
		// TODO maybe NOT! UIGameplay in services...
		App.Instance.Services
			.Get<UIService>()
			.Get<UIGameplay>()
			.statsBehaviour
			.AddReward(reward, target.GetRectTransformCollecting());
	}
	
	private void GameAnimation(bool value)
	{
		_uiCanvasGroup.blocksRaycasts = !value;

		_uiEvents.SlotAnimation?.Invoke(value);
	}
}