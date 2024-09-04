using Game.Profile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameScreen : UIScreen
{
	
	[SerializeField]
	private Button m_settingsButton;

	[SerializeField]
	private UIEnergyPanel m_energyPanel;
	[SerializeField]
	private UILevelProgressPanel m_levelProgressPanel;

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

		_gameplayEvents.StatsEnergyUpdate += OnStatsEnergyUpdate;
	}
	private void OnDisable()
	{
		m_settingsButton.onClick.RemoveListener(SettingsButtonOnClick);

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
		m_levelProgressPanel.SetLevel(value);
		m_levelProgressPanel.SetProgress((float)_playerData.progress.points / _stats.GetCurrentLevelData().points);
		//m_levelText.text = $"level\n{value}"; ;
	}

	private void SettingsButtonOnClick()
	{
		_controller.Show<UISettingsScreen>();
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