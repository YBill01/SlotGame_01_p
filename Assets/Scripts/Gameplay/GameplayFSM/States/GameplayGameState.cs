using UnityEngine;
using YB.HFSM;

public class GameplayGameState : State
{
	private GameConfigData _gameConfig;

	private GameplayEventsService _gameplayEvents;
	private UIEventsService _uiEvents;

	private Game _game;

	public GameplayGameState(GameConfigData gameConfig, GameplayEventsService gameplayEvents, UIEventsService uiEvents)
	{
		_gameConfig = gameConfig;

		_gameplayEvents = gameplayEvents;
		_uiEvents = uiEvents;
	}

	protected override void OnEnter()
	{
		_game = new Game(_gameConfig, _gameplayEvents, _uiEvents);
		_game.Init();

		App.Instance.Services
			.Get<UIService>()
			.Get<UISoundService>()
			.PlayMusic(0);

		UnityEngine.Debug.Log($"GameplayGameState");
	}
	protected override void OnExit()
	{
		_game.Dispose();
		_game = null;

		App.Instance.Services
			.Get<UIService>()
			.Get<UISoundService>()
			.StopMusic();
	}

	protected override void OnUpdate()
	{
		_game.OnUpdate(Time.deltaTime);
	}
}