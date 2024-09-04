using System;
using UnityEngine;

[DefaultExecutionOrder(-9)]
public abstract class UIServiceComponent : MonoBehaviour
{
	public static event Action<UIServiceComponent> Initialized;
	public static event Action<UIServiceComponent> Disposed;

	protected virtual void Awake()
	{
		Initialized?.Invoke(this);

		Initialize();
	}
	protected virtual void OnDestroy()
	{
		Disposed?.Invoke(this);

		Dispose();
	}

	protected virtual void Initialize() { }
	protected virtual void Dispose() { }
}