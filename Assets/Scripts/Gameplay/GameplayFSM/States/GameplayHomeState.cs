using Game.Profile;
using System;
using YB.HFSM;

public class GameplayHomeState : State
{

	/*private UIHome _uiHome;*/
	private UIEvenetsService _uiEvents;

	public GameplayHomeState(/*UIHome uiHome,*/ UIEvenetsService uiEvents)
	{
		/*_uiHome = uiHome;*/
		_uiEvents = uiEvents;
	}

	protected override void OnEnter()
	{
		_uiEvents.Exit += UIExit;
		_uiEvents.SoundOn += UISoundOn;
		_uiEvents.MusicOn += UIMusicOn;

		UnityEngine.Debug.Log($"GameplayHomeState");
	}

	

	

	protected override void OnExit()
	{
		_uiEvents.Exit -= UIExit;
		_uiEvents.SoundOn -= UISoundOn;
		_uiEvents.MusicOn -= UIMusicOn;

	}

	private void UIExit()
	{
		App.Instance.Quit();
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