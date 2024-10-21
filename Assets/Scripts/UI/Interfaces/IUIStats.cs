using UnityEngine;

public interface IUIStats
{
	void SetAmount(int amount);
	void Add(int count);
	void Take(int count);

	RectTransform GetRectTransformCollecting();
}