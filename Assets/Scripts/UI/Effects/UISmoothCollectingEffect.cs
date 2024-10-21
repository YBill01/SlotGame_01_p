using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UISmoothCollectingEffect : MonoBehaviour
{
	[SerializeField]
	private UISmoothCollectingItemEffect m_prefabItem;

	[Space]
	[SerializeField]
	private float m_delay = 0.1f;
	[SerializeField]
	private float m_duration = 1.0f;
	[SerializeField]
	private Ease m_ease = Ease.OutQuad;


	private List<StackInfo> _stacks;




	private bool _isShowingProcess;


	private Tween _tween;

	public struct StackInfo
	{
		public ItemInfo[] items;
		//public int multiplier;

		public Rect startRect;
		public Rect endRect;
	}

	public struct ItemInfo
	{
		public Sprite icon;

		public int amount;

		public Action<int> action;
	}

	private void Awake()
	{
		_stacks = new List<StackInfo>();
	}

	public void Show(StackInfo[] stacks)
	{
		for (int i = 0; i < stacks.Length; i++)
		{
			_stacks.Add(stacks[i]);
		}
		
		if (!_isShowingProcess)
		{
			ShowProcess().Forget();
		}

		_isShowingProcess = true;
	}

	private async UniTaskVoid ShowProcess()
	{
		await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, destroyCancellationToken);

		for (int i = 0; i < _stacks.Count; i++)
		{
			Move(_stacks[i], (m_delay * i), null).Forget();
		}

		_stacks.Clear();
		_isShowingProcess = false;
	}


	private async UniTaskVoid Move(StackInfo stack, float delay, Action action = null)
	{
		await UniTask.WaitForSeconds(delay, cancellationToken: destroyCancellationToken);

		for (int i = 0; i < stack.items.Length; i++)
		{
			ItemInfo item = stack.items[i];

			Vector2 startPoint = RandomUtils.PointInRect(stack.startRect);
			Vector2 endPoint = RandomUtils.PointInRect(stack.endRect);

			RectTransform particleRectTransform = (RectTransform)Instantiate(m_prefabItem, startPoint, m_prefabItem.transform.rotation, transform).transform;
			particleRectTransform.anchoredPosition3D = startPoint;

			CanvasGroup particleCanvasGroup = particleRectTransform.gameObject.GetComponent<CanvasGroup>();

			UISmoothCollectingItemEffect particleCollectingItem = particleRectTransform.gameObject.GetComponent<UISmoothCollectingItemEffect>();
			particleCollectingItem.SetIcon(item.icon);
			particleCollectingItem.Appear();

			float distance = Vector2.Distance(endPoint, startPoint);
			_tween = particleRectTransform.DOAnchorPos(endPoint, m_duration)
				.From(startPoint)
				.SetDelay(0.05f)
				.SetEase(m_ease)
				.OnUpdate(() =>
				{
					float distanceRatio = Vector2.Distance(particleRectTransform.anchoredPosition, startPoint) / distance;
					particleCanvasGroup.alpha = (1.0f - Math.Max(0.75f, distanceRatio)) / (1.0f - 0.75f);
				})
				.OnComplete(() =>
				{
					item.action?.Invoke(item.amount);

					Destroy(particleRectTransform.gameObject);

					App.Instance.Services
						.Get<UIService>()
						.Get<UISoundService>()
						.PlaySFXOnceShot(8, true);
				});
		}

		action?.Invoke();
	}

	private void OnDestroy()
	{
		_tween?.Kill();
	}
}