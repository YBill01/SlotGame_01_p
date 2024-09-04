using System;

public class UIEvenetsService : IService
{
	public Action GoToPlay;
	public Action BackToHome;
	public Action Exit;
	public Action<bool> SoundOn;
	public Action<bool> MusicOn;
}