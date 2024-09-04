using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameplay : UIServiceComponent
{
	[SerializeField]
	private UIScreenController m_uiController;

	private UIEvenetsService _uiEvenetsService;


	protected override void Initialize()
	{
		_uiEvenetsService = App.Instance.Services.Get<UIEvenetsService>();
	}

	private void OnEnable()
	{
		m_uiController.OnShow.AddListener<UIGameScreen>(UIGameScreenOnShow);
		m_uiController.OnHide.AddListener<UIGameScreen>(UIGameScreenOnHide);

		m_uiController.OnShow.AddListener<UISettingsScreen>(UISettingsScreenOnShow);
		m_uiController.OnHide.AddListener<UISettingsScreen>(UISettingsScreenOnHide);
	}
	private void OnDisable()
	{
		m_uiController.OnShow.RemoveListener<UIGameScreen>();
		m_uiController.OnHide.RemoveListener<UIGameScreen>();

		m_uiController.OnShow.RemoveListener<UISettingsScreen>();
		m_uiController.OnHide.RemoveListener<UISettingsScreen>();
	}

	private void UIGameScreenOnShow(UIScreen screen)
	{
		UIGameScreen gameScreen = screen as UIGameScreen;

		
	}
	private void UIGameScreenOnHide(UIScreen screen)
	{
		UIGameScreen gameScreen = screen as UIGameScreen;

		
	}

	private void UISettingsScreenOnShow(UIScreen screen)
	{
		UISettingsScreen settingsScreen = screen as UISettingsScreen;

		settingsScreen.SetPlayMode();
		settingsScreen.Back += UISettingsScreenBack;
		settingsScreen.SoundOn += UISettingsScreenSoundOn;
		settingsScreen.MusicOn += UISettingsScreenMusicOn;
	}
	private void UISettingsScreenOnHide(UIScreen screen)
	{
		UISettingsScreen settingsScreen = screen as UISettingsScreen;

		settingsScreen.Back -= UISettingsScreenBack;
		settingsScreen.SoundOn -= UISettingsScreenSoundOn;
		settingsScreen.MusicOn -= UISettingsScreenMusicOn;
	}

	private void UISettingsScreenBack()
	{
		_uiEvenetsService.BackToHome?.Invoke();
	}
	private void UISettingsScreenSoundOn(bool value)
	{
		_uiEvenetsService.SoundOn?.Invoke(value);
	}
	private void UISettingsScreenMusicOn(bool value)
	{
		_uiEvenetsService.MusicOn?.Invoke(value);
	}


}