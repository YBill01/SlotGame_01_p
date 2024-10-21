using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIHome : UIServiceComponent
{
	[SerializeField]
	private UIScreenController m_uiController;

	[SerializeField]
	private Animator m_personageAnimator;

	private UIEventsService _uiEvenetsService;

	private CanvasGroup _canvasGroup;

	protected override void Initialize()
	{
		_canvasGroup = GetComponent<CanvasGroup>();
		_canvasGroup.blocksRaycasts = true;

		_uiEvenetsService = App.Instance.Services.Get<UIEventsService>();
	}

	private void OnEnable()
	{
		m_uiController.OnShow.AddListener<UIHomeScreen>(UIHomeScreenOnShow);
		m_uiController.OnHide.AddListener<UIHomeScreen>(UIHomeScreenOnHide);

		m_uiController.OnShow.AddListener<UISettingsScreen>(UISettingsScreenOnShow);
		m_uiController.OnHide.AddListener<UISettingsScreen>(UISettingsScreenOnHide);
	}
	private void OnDisable()
	{
		m_uiController.OnShow.RemoveListener<UIHomeScreen>();
		m_uiController.OnHide.RemoveListener<UIHomeScreen>();

		m_uiController.OnShow.RemoveListener<UISettingsScreen>();
		m_uiController.OnHide.RemoveListener<UISettingsScreen>();
	}

	private void UIHomeScreenOnShow(UIScreen screen)
	{
		UIHomeScreen homeScreen = screen as UIHomeScreen;

		homeScreen.Play += UIHomeScreenPlay;
	}
	private void UIHomeScreenOnHide(UIScreen screen)
	{
		UIHomeScreen homeScreen = screen as UIHomeScreen;

		homeScreen.Play -= UIHomeScreenPlay;
	}

	private void UISettingsScreenOnShow(UIScreen screen)
	{
		UISettingsScreen settingsScreen = screen as UISettingsScreen;

		settingsScreen.SetHomeMode();
		settingsScreen.Exit += UISettingsScreenExit;
		settingsScreen.SoundOn += UISettingsScreenSoundOn;
		settingsScreen.MusicOn += UISettingsScreenMusicOn;
	}
	private void UISettingsScreenOnHide(UIScreen screen)
	{
		UISettingsScreen settingsScreen = screen as UISettingsScreen;

		settingsScreen.Exit -= UISettingsScreenExit;
		settingsScreen.SoundOn -= UISettingsScreenSoundOn;
		settingsScreen.MusicOn -= UISettingsScreenMusicOn;
	}

	private void UIHomeScreenPlay()
	{
		PlayProcess().Forget();
	}

	private async UniTaskVoid PlayProcess()
	{
		_canvasGroup.blocksRaycasts = false;
		m_personageAnimator.SetTrigger("Go");

		await UniTask.WaitForSeconds(1.8f);

		_uiEvenetsService.GoToPlay?.Invoke();
	}

	private void UISettingsScreenExit()
	{
		_uiEvenetsService.Exit?.Invoke();
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