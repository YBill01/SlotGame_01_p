using System;
using TMPro;
using UnityEngine;

public class UIMatch3Screen : UIGameScreen
{
	[Space]
	[SerializeField]
	private UIMatch3Game m_game;

	[Space]
	[SerializeField]
	private TMP_Text m_playTimerText;

	private Match3ConfigData _config;
	private GameplayEventsService _gameplayEvents;
	private UIEventsService _uiEvents;

	private DateTime _playTimeEnd;
	private bool _isPlaying;

	protected override void OnInit()
	{
		base.OnInit();

		_config = App.Instance.Gameplay.Stats.GetCurrentLevelData().match3Config;
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

		m_game.onRewardDrop += RewardDrop;
	}
	private void OnDisable()
	{
		_gameplayEvents.GameRestart -= GameRestart;

		m_game.onRewardDrop -= RewardDrop;
	}

	private void Update()
	{
		if (_isPlaying)
		{
			TimeSpan timeSpan = _playTimeEnd - DateTime.UtcNow;

			m_playTimerText.text = timeSpan.ToString(@"mm\:ss");

			if (timeSpan.TotalSeconds <= 0)
			{
				_isPlaying = false;

				_gameplayEvents.GameSlot?.Invoke();
			}
		}
	}

	private void GameRestart()
	{
		_config = App.Instance.Gameplay.Stats.GetCurrentLevelData().match3Config;

		m_game.Init(_config, _gameplayEvents, _uiEvents);

		_playTimeEnd = DateTime.UtcNow.AddSeconds(_config.gameTime);
		_isPlaying = true;

		m_game.StartGame();
	}

	private void RewardDrop(RewardData[] reward, IUIStatsDrop target)
	{
		// TODO maybe NOT! UIGameplay in services...
		App.Instance.Services
			.Get<UIService>()
			.Get<UIGameplay>()
			.statsBehaviour
			.AddReward(reward, target.GetRectTransformCollecting());
	}
}