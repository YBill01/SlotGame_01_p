using SlotGame.Profile;
using System;
using UnityEngine;

public class App
{
	public static App Instance { get; private set; }

	public ServiceLocator Services { get; private set; }

	public Gameplay Gameplay { get; private set; }

	public static class Scenes
	{
		public const int BOOTSTRAP	 = 0;
		public const int LOADING	 = 1;
		public const int GAMEPLAY	 = 2;
		public const int HOME		 = 3;
		public const int GAME		 = 4;
	}

	public App(ServiceLocator services)
	{
		if (Instance != null && Instance != this)
		{
			throw new Exception("An instance of this singleton already exists.");
		}

		Instance = this;

		Services = services;

		Init();
	}

	private void Init()
	{
		AppData appData = Profile.Instance.Get<AppData>().Load();
		appData.lastEntryDate = DateTime.UtcNow;
		PlayerData playerData = Profile.Instance.Get<PlayerData>().Load();

		//test...
		//appData.firstPlay = false;
		//playerData.progress.level = 3;
		//playerData.progress.points = 5;

		//playerData.stats.energy = 2;
		//playerData.energyRecovery.startTime = new DateTime(1725427118);
		//playerData.energyRecovery.endTime = new DateTime(1725427118 + 130);
		//.......

		Services.Get<GameplayEventsService>().Initialized += GameplayOnInitialized;
	}

	private void GameplayOnInitialized(Gameplay gameplay)
	{
		Gameplay = gameplay;

		Services.Get<UIService>()
			.Get<UILoader>()
			.LoadScene(App.Scenes.HOME);
	}

	public void Quit()
	{
#if UNITY_EDITOR
		App.Instance = null;
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
		//System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
	}
}