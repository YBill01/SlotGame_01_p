using YB.HFSM;

public class GameSlotState : State
{
	private Game _game;
	//private SlotConfigData _config;
	private GameplayEventsService _gameplayEvents;
	private UIEventsService _uiEvents;

	private SlotMachine _slot;

	public GameSlotState(Game game, GameplayEventsService gameplayEvents, UIEventsService uiEvents)
	{
		_game = game;
		_gameplayEvents = gameplayEvents;
		_uiEvents = uiEvents;
	}

	protected override void OnEnter()
	{
		//_config = App.Instance.Gameplay.Stats.GetCurrentLevelData().slotConfig;

		_uiEvents.SlotStart += SlotStart;
		_uiEvents.SlotSpin += SlotSpin;
		_uiEvents.SlotEndSpin += SlotEndSpin;

		_slot = new SlotMachine(_gameplayEvents);
		
		UnityEngine.Debug.Log("Game state::Slot");
	}
	protected override void OnExit()
	{
		_uiEvents.SlotStart -= SlotStart;
		_uiEvents.SlotSpin -= SlotSpin;
		_uiEvents.SlotEndSpin -= SlotEndSpin;

		_slot.Dispose();
		_slot = null;
	}

	private void SlotStart()
	{
		_slot.Start(App.Instance.Gameplay.Stats.GetCurrentLevelData().slotConfig);
	}
	private void SlotSpin()
	{
		_slot.Spin();
	}
	private void SlotEndSpin()
	{
		_slot.CheckSpinResult();
	}
}