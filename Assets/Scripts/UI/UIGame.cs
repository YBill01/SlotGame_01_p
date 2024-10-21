using UnityEngine;

public class UIGame : UIServiceComponent
{
	[SerializeField]
	private UIScreenController m_uiController;

	private GameplayEventsService _gameplayEvents;
	private UIEventsService _uiEvenetsService;


	public Game CurrentGame { get; private set; }
	public enum Game
	{
		Slot,
		Match3
	}

	protected override void Initialize()
	{
		_gameplayEvents = App.Instance.Services.Get<GameplayEventsService>();
		_uiEvenetsService = App.Instance.Services.Get<UIEventsService>();
	}

	private void OnEnable()
	{
		_gameplayEvents.GameSlot += OpenGameSlotScreen;
		_gameplayEvents.GameMatch3 += OpenGameMatch3Screen;
	}
	private void OnDisable()
	{
		_gameplayEvents.GameSlot -= OpenGameSlotScreen;
		_gameplayEvents.GameMatch3 -= OpenGameMatch3Screen;
	}


	private void OpenGameSlotScreen()
	{
		CurrentGame = Game.Slot;
		m_uiController.Show<UISlotMachineScreen>();
	}
	private void OpenGameMatch3Screen()
	{
		CurrentGame = Game.Match3;
		m_uiController.Show<UIMatch3Screen>();
	}

}