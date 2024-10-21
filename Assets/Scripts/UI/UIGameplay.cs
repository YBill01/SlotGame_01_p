using UnityEngine;

[RequireComponent(typeof(UIStatsBehaviour))]
public class UIGameplay : UIServiceComponent
{
	[SerializeField]
	private UIScreenController m_uiController;

	[HideInInspector]
	public UIStatsBehaviour statsBehaviour;

	private GameplayEventsService _gameplayEvents;
	private UIEventsService _uiEvenetsService;

	private UIGameplayScreen _gameplayScreen;

	protected override void Initialize()
	{
		_gameplayEvents = App.Instance.Services.Get<GameplayEventsService>();
		_uiEvenetsService = App.Instance.Services.Get<UIEventsService>();

		statsBehaviour = GetComponent<UIStatsBehaviour>();
	}

	private void OnEnable()
	{
		statsBehaviour.onShopOpen += OpenShopScreen;

		m_uiController.OnShow.AddListener<UIGameplayScreen>(UIGameScreenOnShow);
		m_uiController.OnHide.AddListener<UIGameplayScreen>(UIGameScreenOnHide);

		m_uiController.OnShow.AddListener<UIShopScreen>(UIShopScreenOnShow);
		m_uiController.OnHide.AddListener<UIShopScreen>(UIShopScreenOnHide);

		m_uiController.OnShow.AddListener<UISettingsScreen>(UISettingsScreenOnShow);
		m_uiController.OnHide.AddListener<UISettingsScreen>(UISettingsScreenOnHide);
	}
	private void OnDisable()
	{
		statsBehaviour.onShopOpen -= OpenShopScreen;

		m_uiController.OnShow.RemoveListener<UIGameplayScreen>();
		m_uiController.OnHide.RemoveListener<UIGameplayScreen>();

		m_uiController.OnShow.RemoveListener<UIShopScreen>();
		m_uiController.OnHide.RemoveListener<UIShopScreen>();

		m_uiController.OnShow.RemoveListener<UISettingsScreen>();
		m_uiController.OnHide.RemoveListener<UISettingsScreen>();
	}

	private void UIGameScreenOnShow(UIScreen screen)
	{
		UIGameplayScreen gameplayScreen = screen as UIGameplayScreen;

		_gameplayScreen = gameplayScreen;
	}
	private void UIGameScreenOnHide(UIScreen screen)
	{
		UIGameplayScreen gameplayScreen = screen as UIGameplayScreen;

		_gameplayScreen = null;
	}

	private void UIShopScreenOnShow(UIScreen screen)
	{
		UIShopScreen shopScreen = screen as UIShopScreen;

		if (_gameplayScreen != null)
		{
			_gameplayScreen.ToolboxVisible(false);
		}
	}
	private void UIShopScreenOnHide(UIScreen screen)
	{
		UIShopScreen shopScreen = screen as UIShopScreen;

		if (_gameplayScreen != null)
		{
			_gameplayScreen.ToolboxVisible(true);
		}
	}

	private void UISettingsScreenOnShow(UIScreen screen)
	{
		UISettingsScreen settingsScreen = screen as UISettingsScreen;

		settingsScreen.SetPlayMode();
		settingsScreen.Back += UISettingsScreenBack;
		settingsScreen.Info += UISettingsScreenInfo;
		settingsScreen.SoundOn += UISettingsScreenSoundOn;
		settingsScreen.MusicOn += UISettingsScreenMusicOn;
	}
	private void UISettingsScreenOnHide(UIScreen screen)
	{
		UISettingsScreen settingsScreen = screen as UISettingsScreen;

		settingsScreen.Back -= UISettingsScreenBack;
		settingsScreen.Info -= UISettingsScreenInfo;
		settingsScreen.SoundOn -= UISettingsScreenSoundOn;
		settingsScreen.MusicOn -= UISettingsScreenMusicOn;
	}

	private void UISettingsScreenBack()
	{
		_uiEvenetsService.BackToHome?.Invoke();
	}
	private void UISettingsScreenInfo()
	{
		m_uiController.Show<UIInfoScreen>();
	}
	private void UISettingsScreenSoundOn(bool value)
	{
		_uiEvenetsService.SoundOn?.Invoke(value);
	}
	private void UISettingsScreenMusicOn(bool value)
	{
		_uiEvenetsService.MusicOn?.Invoke(value);
	}

	private void OpenShopScreen()
	{
		if (App.Instance.Services.Get<UIService>().Get<UIGame>().CurrentGame != UIGame.Game.Match3)
		{
			m_uiController.Show<UIShopScreen>();
		}
	}
}