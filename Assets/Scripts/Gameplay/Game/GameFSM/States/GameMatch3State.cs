using UnityEngine;
using YB.HFSM;

public class GameMatch3State : State
{
	private Game _game;
	//private Match3ConfigData _config;
	private GameplayEventsService _gameplayEvents;
	private UIEventsService _uiEvents;

	private Match3 _match3;

	public GameMatch3State(Game game, GameplayEventsService gameplayEvents, UIEventsService uiEvents)
	{
		_game = game;
		_gameplayEvents = gameplayEvents;
		_uiEvents = uiEvents;
	}

	protected override void OnEnter()
	{
		//_config = App.Instance.Gameplay.Stats.GetCurrentLevelData().match3Config;

		_uiEvents.Match3Start += Match3Start;
		_uiEvents.TryMatch3MoveItem += TryMatch3MoveItem;
		_uiEvents.Match3FindMatches += Match3FindMatches;
		_uiEvents.Match3FallingItems += Match3FallingItems;
		_uiEvents.Match3FillingItems += Match3FillingItems;

		_match3 = new Match3(_gameplayEvents);
		
		UnityEngine.Debug.Log("Game state::Match3");
	}
	protected override void OnExit()
	{
		_uiEvents.Match3Start -= Match3Start;
		_uiEvents.TryMatch3MoveItem -= TryMatch3MoveItem;
		_uiEvents.Match3FindMatches -= Match3FindMatches;
		_uiEvents.Match3FallingItems -= Match3FallingItems;
		_uiEvents.Match3FillingItems -= Match3FillingItems;

		_match3.Dispose();
		_match3 = null;
	}

	private void Match3Start()
	{
		_match3.Start(App.Instance.Gameplay.Stats.GetCurrentLevelData().match3Config);
	}
	private bool TryMatch3MoveItem(Match3.ItemInfo itemInfo, Vector2Int axisDirection)
	{
		return _match3.TryMoveItem(itemInfo, axisDirection);
	}
	private void Match3FindMatches()
	{
		_match3.MatchesProcess();
	}
	private void Match3FallingItems()
	{
		_match3.FallingItemsProcess();
	}
	private void Match3FillingItems()
	{
		_match3.FillingItems();
	}
}