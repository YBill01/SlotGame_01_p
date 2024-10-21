using Cysharp.Threading.Tasks;
using System;

public class Game : IDisposable
{
	private GameConfigData _config;
	private GameplayEventsService _gameplayEvents;
	private UIEventsService _uiEvents;

	private GameStateMachine _gameFSM;

	private Action _gameMatch3;
	private Action _gameSlot;

	public Game(GameConfigData config, GameplayEventsService gameplayEvents, UIEventsService uiEvents)
	{
		_config = config;
		_gameplayEvents = gameplayEvents;
		_uiEvents = uiEvents;

		GameSlotState slotState = new GameSlotState(this, _gameplayEvents, _uiEvents);
		GameMatch3State match3State = new GameMatch3State(this, _gameplayEvents, _uiEvents);

		_gameFSM = new GameStateMachine(slotState, match3State);

		_gameMatch3 = slotState.AddEventTransition(match3State);
		_gameSlot = match3State.AddEventTransition(slotState);
	}

	public void Init()
	{
		_gameFSM.Init();

		_gameplayEvents.GameMatch3 += _gameMatch3;
		_gameplayEvents.GameSlot += _gameSlot;

		GameStartProcess().Forget();
	}

	private async UniTaskVoid GameStartProcess()
	{
		await UniTask.NextFrame();
		//await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
		
		_gameplayEvents.GameSlot?.Invoke();
		//_gameplayEvents.GameMatch3?.Invoke();
	}

	public void OnUpdate(float deltaTime)
	{
		_gameFSM.Update();
	}

	public void Dispose()
	{
		_gameplayEvents.GameMatch3 -= _gameMatch3;
		_gameplayEvents.GameSlot -= _gameSlot;

		_gameFSM.Dispose();
	}
}