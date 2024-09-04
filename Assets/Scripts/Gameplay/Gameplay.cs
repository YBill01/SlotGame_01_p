using Game.Profile;
using System;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
	[SerializeField]
	private SlotConfigData m_slotConfig;

	private GameplayStateMachine _gameplayFSM;

	public StatsBehaviour Stats { get; private set; }

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);

		GameplayEventsService gameplayEvents = App.Instance.Services.Get<GameplayEventsService>();

		//UIHome uiHome = App.Instance.Services.Get<UIService>().Get<UIHome>();
		UIEvenetsService uiEvents = App.Instance.Services.Get<UIEvenetsService>();

		uiEvents.GoToPlay += UIGoToPlay;
		uiEvents.BackToHome += UIBackToHome;

		GameplayHomeState gameplayHomeState = new GameplayHomeState(/*uiHome,*/ uiEvents);
		GameplayGameState gameplayGameState = new GameplayGameState(uiEvents);

		_gameplayFSM = new GameplayStateMachine(gameplayHomeState, gameplayGameState);

		gameplayEvents.Home += gameplayGameState.AddEventTransition(gameplayHomeState);
		gameplayEvents.Game += gameplayHomeState.AddEventTransition(gameplayGameState);

		gameplayEvents.Initialized?.Invoke(this);
	}

	private void UIGoToPlay()
	{
		App.Instance.Services.Get<UIService>()
			.Get<UILoader>()
			.LoadScene(App.Scenes.GAME, App.Instance.Services.Get<GameplayEventsService>().Game);
	}
	private void UIBackToHome()
	{
		App.Instance.Services.Get<UIService>()
			.Get<UILoader>()
			.LoadScene(App.Scenes.HOME, App.Instance.Services.Get<GameplayEventsService>().Home);
	}

	private void Start()
	{
		Stats = new StatsBehaviour(m_slotConfig);

		_gameplayFSM.Init();
	}

	private void Update()
	{
		Stats.Update(Time.deltaTime);

		_gameplayFSM.Update();
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