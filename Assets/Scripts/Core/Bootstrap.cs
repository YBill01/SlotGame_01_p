using Cysharp.Threading.Tasks;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void Initialize()
	{
		Application.targetFrameRate = 60;
		Application.runInBackground = true;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	private async UniTaskVoid Start()
	{
		//DontDestroyOnLoad(gameObject);

		await UniTask.NextFrame();

		ServiceLocator services = new ServiceLocator();

		GameplayEventsService gameplayEvents = services.Add(new GameplayEventsService());
		UIEventsService uiEvents = services.Add(new UIEventsService());
		LoaderService loader = services.Add(new LoaderService());
		UIService ui = services.Add(new UIService());

		App app = new App(services);

		await UniTask.NextFrame();

		Init();
	}

	private void Init()
	{
		App.Instance.Services.Get<LoaderService>()
			.LoadScene(App.Scenes.LOADING)
			.OnComplete(() =>
			{
				App.Instance.Services.Get<LoaderService>()
					.LoadScene(App.Scenes.GAMEPLAY);
			});
	}
}