using Game.Profile;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHomeScreen : UIScreen
{
	public event Action Play;

	//[SerializeField]
	//private Animator m_personageAnimator;

	[SerializeField]
	private Button m_settingsButton;
	[SerializeField]
	private Button m_playButton;

	[SerializeField]
	private UIEnergyPanel m_energyPanel;

	[SerializeField]
	private TMP_Text m_levelText;


	private PlayerData _playerData;
	private StatsBehaviour _stats;
	private GameplayEventsService _gameplayEvents;

	private void Awake()
	{
		_playerData = Profile.Instance.Get<PlayerData>().data;
		_stats = App.Instance.Gameplay.Stats;
		_gameplayEvents = App.Instance.Services.Get<GameplayEventsService>();
	}

	private void OnEnable()
	{
		m_settingsButton.onClick.AddListener(SettingsButtonOnClick);
		m_playButton.onClick.AddListener(PlayButtonOnClick);

		_gameplayEvents.StatsEnergyUpdate += OnStatsEnergyUpdate;
	}
	private void OnDisable()
	{
		m_settingsButton.onClick.RemoveListener(SettingsButtonOnClick);
		m_playButton.onClick.RemoveListener(PlayButtonOnClick);

		_gameplayEvents.StatsEnergyUpdate -= OnStatsEnergyUpdate;
	}

	

	private void Update()
	{
		if (_stats.IsEnergyRecovery)
		{
			float value = 1.0f - (float)((_playerData.energyRecovery.endTime - DateTime.UtcNow).TotalSeconds / _stats.GetCurrentLevelData().energy.recoveryCooldown);
			m_energyPanel.SetRecoveryProgress(value);
		}
	}

	protected override void OnShow()
	{
		SetLevel(_playerData.progress.level);
		m_energyPanel.SetAmount(_playerData.stats.energy, (float)_playerData.stats.energy / _stats.GetCurrentLevelData().energy.max);
		m_energyPanel.SetRecoveryProgress(0.0f);
	}

	public void SetLevel(int value)
	{
		m_levelText.text = $"level\n{value}"; ;
	}

	private void SettingsButtonOnClick()
	{
		_controller.Show<UISettingsScreen>();
	}

	private void PlayButtonOnClick()
	{
		Play?.Invoke();

		//m_personageAnimator.SetTrigger("Go");

		/*App.Instance.Services
				.Get<UIService>()
				.Get<UILoader>()
				.LoadScene(App.Scenes.GAME);*/
	}


	private void OnStatsEnergyUpdate(int count, bool isAdded)
	{
		if (isAdded)
		{
			m_energyPanel.Add(count);
		}
		else
		{
			m_energyPanel.Take(count);
		}

		m_energyPanel.SetAmount(_playerData.stats.energy, (float)_playerData.stats.energy / _stats.GetCurrentLevelData().energy.max);
	}

}