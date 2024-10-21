using UnityEngine;

public class UIStatsDropComponent : MonoBehaviour, IUIStatsDrop
{
	public RectTransform GetRectTransformCollecting()
	{
		return (RectTransform)transform;
	}
}