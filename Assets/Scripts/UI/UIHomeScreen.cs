using SlotGame.Profile;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHomeScreen : UIScreen
{
	public event Action Play;

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
	}
	private void OnDisable()
	{
		m_settingsButton.onClick.RemoveListener(SettingsButtonOnClick);
		m_playButton.onClick.RemoveListener(PlayButtonOnClick);
	}

	private void Start()
	{
		m_levelText.text = $"level\n{_playerData.progress.level}";
	}

	protected override void OnShow()
	{
		
	}

	private void SettingsButtonOnClick()
	{
		_controller.Show<UISettingsScreen>();
	}

	private void PlayButtonOnClick()
	{
		Play?.Invoke();
	}
}