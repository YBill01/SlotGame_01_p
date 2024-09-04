using Game.Profile;
using YB.HFSM;

public class GameplayGameState : State
{

	private UIEvenetsService _uiEvents;

	public GameplayGameState(UIEvenetsService uiEvents)
	{
		_uiEvents = uiEvents;
	}

	protected override void OnEnter()
	{

		_uiEvents.SoundOn += UISoundOn;
		_uiEvents.MusicOn += UIMusicOn;

		UnityEngine.Debug.Log($"GameplayGameState");
	}
	protected override void OnExit()
	{
		_uiEvents.SoundOn -= UISoundOn;
		_uiEvents.MusicOn -= UIMusicOn;
	}



	private void UISoundOn(bool value)
	{
		Profile.Instance.Get<AppData>().data.soundOn = value;
	}
	private void UIMusicOn(bool value)
	{
		Profile.Instance.Get<AppData>().data.musicOn = value;
	}

}