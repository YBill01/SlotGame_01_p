using System;
using System.Collections.Generic;

public class UIService : IService
{
	private Dictionary<Type, UIServiceComponent> _dic;

	public UIService()
	{
		_dic = new Dictionary<Type, UIServiceComponent>();

		UIServiceComponent.Initialized += OnInizializeComponent;
		UIServiceComponent.Disposed += OnDisposeComponent;
	}

	private void OnInizializeComponent(UIServiceComponent component)
	{
		TryAddComponent(component);
	}
	private void OnDisposeComponent(UIServiceComponent component)
	{
		TryRemoveComponent(component);
	}

	private bool TryAddComponent(UIServiceComponent component)
	{
		return _dic.TryAdd(component.GetType(), component);
	}
	private bool TryRemoveComponent(UIServiceComponent component)
	{
		return _dic.Remove(component.GetType());
	}

	public bool Has<T>() where T : UIServiceComponent
	{
		return _dic.ContainsKey(typeof(T));
	}
	public T Get<T>() where T : UIServiceComponent
	{
		if (_dic.TryGetValue(typeof(T), out UIServiceComponent value))
		{
			return (T)value;
		}
		else
		{
			throw new ArgumentException($"<color=red>UI Component: '{typeof(T).Name}' is not found.</color>");
		}
	}
}