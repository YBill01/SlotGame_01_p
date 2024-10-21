using SlotGame.Profile;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UIGameplayScreen : UIScreen
{
	[Space]
	[SerializeField]
	private Button m_settingsButton;

	[Space]
	[SerializeField]
	private UIToolboxPanel m_toolboxPanel;

	private PlayerData _playerData;
	private StatsBehaviour _stats;
	private GameplayEventsService _gameplayEvents;
	private UIEventsService _uiEvents;

	protected CanvasGroup _canvasGroup;

	private void Awake()
	{
		_playerData = Profile.Instance.Get<PlayerData>().data;
		_stats = App.Instance.Gameplay.Stats;
		_gameplayEvents = App.Instance.Services.Get<GameplayEventsService>();
		_uiEvents = App.Instance.Services.Get<UIEventsService>();

		_canvasGroup = GetComponent<CanvasGroup>();
	}

	private void OnEnable()
	{
		_gameplayEvents.GameSlot += GameSlot;
		_gameplayEvents.GameMatch3 += GameMatch3;

		_gameplayEvents.StatsLevelUp += StatsLevelUp;

		_uiEvents.SlotAnimation += SlotAnimation;

		m_settingsButton.onClick.AddListener(SettingsButtonOnClick);
		m_toolboxPanel.onClick += ToolboxPanelOnClick;
	}
	private void OnDisable()
	{
		_gameplayEvents.GameSlot -= GameSlot;
		_gameplayEvents.GameMatch3 -= GameMatch3;

		_gameplayEvents.StatsLevelUp -= StatsLevelUp;

		_uiEvents.SlotAnimation -= SlotAnimation;

		m_settingsButton.onClick.RemoveListener(SettingsButtonOnClick);
		m_toolboxPanel.onClick -= ToolboxPanelOnClick;
	}

	private void GameSlot()
	{
		ToolboxVisible(true);
	}
	private void GameMatch3()
	{
		ToolboxVisible(false);
	}
	
	private void StatsLevelUp(int currentLevel)
	{
		App.Instance.Services
			.Get<UIService>()
			.Get<UISplash>()
			.OpenLevelUpScreen();

		Debug.Log("LevelUp show...");
	}

	private void SlotAnimation(bool value)
	{
		_canvasGroup.blocksRaycasts = !value;
	}

	public void ToolboxVisible(bool value)
	{
		if (App.Instance.Services.Get<UIService>().Get<UIGame>().CurrentGame != UIGame.Game.Match3)
		{
			m_toolboxPanel.SetVisible(value);
		}
		else
		{
			m_toolboxPanel.SetVisible(false);
		}
	}

	private void SettingsButtonOnClick()
	{
		_controller.Show<UISettingsScreen>();
	}

	private void ToolboxPanelOnClick()
	{
		if (!_stats.IsMatch3Cooldown)
		{
			_gameplayEvents.GameMatch3?.Invoke();
		}
	}
}