using YB.HFSM;

public class GameplayStateMachine : StateMachine
{
	public GameplayStateMachine(params State[] states) : base(states)
	{
	}
}