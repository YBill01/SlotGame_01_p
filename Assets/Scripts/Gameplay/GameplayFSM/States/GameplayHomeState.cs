using YB.HFSM;

public class GameplayHomeState : State
{
	private UIEventsService _uiEvents;

	public GameplayHomeState(UIEventsService uiEvents)
	{
		_uiEvents = uiEvents;
	}

	protected override void OnEnter()
	{
		UnityEngine.Debug.Log($"GameplayHomeState");
	}
	protected override void OnExit()
	{

	}
}