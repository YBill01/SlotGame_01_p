using SlotGame.Profile;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
	[SerializeField]
	private GameConfigData m_gameConfig;
	[SerializeField]
	private GameStatsStartData m_gameStatsStart;

	private GameplayStateMachine _gameplayFSM;

	public StatsBehaviour Stats { get; private set; }

	private bool _isLoading;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);

		GameplayEventsService gameplayEvents = App.Instance.Services.Get<GameplayEventsService>();

		UIEventsService uiEvents = App.Instance.Services.Get<UIEventsService>();

		uiEvents.GoToPlay += UIGoToPlay;
		uiEvents.BackToHome += UIBackToHome;

		GameplayHomeState gameplayHomeState = new GameplayHomeState(uiEvents);
		GameplayGameState gameplayGameState = new GameplayGameState(m_gameConfig, gameplayEvents, uiEvents);

		_gameplayFSM = new GameplayStateMachine(gameplayHomeState, gameplayGameState);

		gameplayEvents.Home += gameplayGameState.AddEventTransition(gameplayHomeState);
		gameplayEvents.Game += gameplayHomeState.AddEventTransition(gameplayGameState);

		uiEvents.SoundOn += UISoundOn;
		uiEvents.MusicOn += UIMusicOn;

		uiEvents.Exit += UIExit;

		gameplayEvents.Initialized?.Invoke(this);
	}

	private void UIGoToPlay()
	{
		/*App.Instance.Services.Get<UIService>()
			.Get<UILoader>()
			.LoadScene(App.Scenes.GAME, App.Instance.Services.Get<GameplayEventsService>().Game);*/

		_isLoading = true;
		App.Instance.Services.Get<UIService>()
			.Get<UILoader>()
			.LoadScene(App.Scenes.GAME, () =>
			{
				App.Instance.Services.Get<GameplayEventsService>().Game?.Invoke();
				_isLoading = false;
			});
	}
	private void UIBackToHome()
	{
		/*App.Instance.Services.Get<UIService>()
			.Get<UILoader>()
			.LoadScene(App.Scenes.HOME, App.Instance.Services.Get<GameplayEventsService>().Home);*/
		_isLoading = true;
		App.Instance.Services.Get<UIService>()
			.Get<UILoader>()
			.LoadScene(App.Scenes.HOME, () =>
			{
				App.Instance.Services.Get<GameplayEventsService>().Home?.Invoke();
				_isLoading = false;
			});
	}

	private void UISoundOn(bool value)
	{
		App.Instance.Services
			.Get<UIService>()
			.Get<UISoundService>()
			.SFXVolume(value ? 1.0f : 0.0001f);

		Profile.Instance.Get<AppData>().data.soundOn = value;
	}
	private void UIMusicOn(bool value)
	{
		App.Instance.Services
			.Get<UIService>()
			.Get<UISoundService>()
			.MusicVolume(value ? 1.0f : 0.0001f);

		Profile.Instance.Get<AppData>().data.musicOn = value;
	}

	private void UIExit()
	{
		App.Instance.Quit();
	}

	private void Start()
	{
		App.Instance.Services
			.Get<UIService>()
			.Get<UISoundService>()
			.MusicVolume(Profile.Instance.Get<AppData>().data.musicOn ? 1.0f : 0.0001f);

		App.Instance.Services
			.Get<UIService>()
			.Get<UISoundService>()
			.SFXVolume(Profile.Instance.Get<AppData>().data.soundOn ? 1.0f : 0.0001f);

		Stats = new StatsBehaviour(m_gameConfig, m_gameStatsStart);

		_gameplayFSM.Init();
	}

	private void Update()
	{
		_gameplayFSM.Update();

		/*if (App.Instance.Services.Get<LoaderService>().State != LoaderService.LoaderState.Loading)
		{
			Stats.Update();
		}*/
		if (!_isLoading)
		{
			Stats.Update();
		}
	}

	private void OnDestroy()
	{
#if UNITY_EDITOR
		Profile.Instance.Get<AppData>().Save();
		Profile.Instance.Get<PlayerData>().Save();

		App.Instance?.Quit();

		//UnityEngine.Debug.Log($"Quit");
#endif
	}

	private void OnApplicationFocus(bool focus)
	{
		if (!focus)
		{
			Profile.Instance.Get<AppData>().Save();
			Profile.Instance.Get<PlayerData>().Save();

			//UnityEngine.Debug.Log($"Quit");
		}
	}
}