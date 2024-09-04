using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderService : IService
{
	public Action<float> LoadProgress;
	public Action LoadComplete;

	public int IndexScene { get; private set; }

	public LoaderState State { get; private set; }
	public enum LoaderState
	{
		None,
		Loading,
		Completed
	}

	public void Clear()
	{
		State = LoaderState.None;
		IndexScene = -1;
	}

	public LoaderService LoadScene(int indexScene, float delay = 0.0f)
	{
		if (State == LoaderState.Loading)
		{
			Debug.LogError($"Loader is already using {LoaderState.Loading} state.");

			return this;
		}
		State = LoaderState.Loading;

		LoadSceneAsync(indexScene, delay).Forget();

		return this;
	}

	private async UniTask LoadSceneAsync(int indexScene, float delay = 0.0f)
	{
		await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
		await SceneManager.LoadSceneAsync(indexScene).ToUniTask(Progress.Create<float>(v => LoadProgress?.Invoke(v)));
		await UniTask.WaitForSeconds(delay);
		await UniTask.NextFrame();

		State = LoaderState.Completed;

		LoadComplete?.Invoke();

		Clear();

		LoadProgress = null;
		LoadComplete = null;
	}
}

public static class LoaderServiceExtensions
{
	public static T OnProgress<T>(this T t, Action<float> action) where T : LoaderService
	{
		t.LoadProgress = action;

		return t;
	}
	public static T OnComplete<T>(this T t, Action action) where T : LoaderService
	{
		t.LoadComplete = action;

		return t;
	}
}