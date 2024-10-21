using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UIMatch3Item : MonoBehaviour, IUIStatsDrop, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField]
	private UIImageEffect m_image;

	[SerializeField]
	private float m_tweenDuration = 1.0f;

	[Space]
	[SerializeField]
	private RectTransform m_dropRectTransform;

	private Match3ItemData _data;

	private RectTransform _rectTransform;

	private Vector2Int _coords;
	private Action<Vector2Int, Vector2Int> _moveItem;

	private void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
	}

	public void Init(Action<Vector2Int, Vector2Int> moveItem)
	{
		_moveItem = moveItem;
	}

	public void SetData(Match3ItemData itemData)
	{
		_data = itemData;

		m_image.GetComponent<Image>().sprite = _data.view.icon;
	}

	public void SetPosition(float positionX, float positionY)
	{
		_rectTransform.anchoredPosition = new Vector2(positionX, positionY);
	}

	public void SetCoords(Vector2Int coords)
	{
		_coords = coords;
	}

	public RectTransform GetRectTransformCollecting()
	{
		return m_dropRectTransform;
	}

	public void DoJumpPosition(float positionX, float positionY)
	{
		Vector2 endValue = new Vector2(positionX, positionY);
		_rectTransform.DOJumpAnchorPos(endValue, 80, 1, m_tweenDuration)
			.OnComplete(() => {
				_rectTransform.anchoredPosition = endValue;
			});
		m_image.ScaleOutEffect();
	}
	public void DoFallPosition(float positionX, float positionY)
	{
		Vector2 endValue = new Vector2(positionX, positionY);
		_rectTransform.DOAnchorPos(endValue, m_tweenDuration)
			.SetEase(Ease.OutBounce)
			.OnComplete(() => {
				_rectTransform.anchoredPosition = endValue;
			});
		m_image.ScaleOutEffect();
	}
	public void DoPosition(float positionX, float positionY)
	{
		Vector2 endValue = new Vector2(positionX, positionY);
		_rectTransform.DOAnchorPos(endValue, m_tweenDuration)
			.SetEase(Ease.OutBack)
			.OnComplete(() => {
				_rectTransform.anchoredPosition = endValue;
			});
		m_image.ScaleOutEffect();
	}

	public void Appear()
	{
		m_image.FlashScaleUpEffect();
		m_image.AlphaInEffect();
	}
	public void Disappear(Action action)
	{
		m_image.FlashScaleUpEffect();
		m_image.AlphaOutEffect(() =>
		{
			action?.Invoke();
		});
	}
	public void WrongMove()
	{
		m_image.ScaleOutEffect();
		m_image.ShakeEffect();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		transform.SetAsLastSibling();
		m_image.ScaleInEffect();
	}
	public void OnPointerUp(PointerEventData eventData)
	{
		MoveProcess(eventData.pressPosition, eventData.position);
	}

	private void MoveProcess(Vector2 startPosition, Vector2 endPosition)
	{
		float distance = Vector2.Distance(startPosition, endPosition);

		if (distance > 0)
		{
			Vector2 targetDirection = endPosition - startPosition;
			Vector2 normalizedDirection = targetDirection.normalized;

			Vector2Int axisDirection;
			if (Mathf.Abs(normalizedDirection.x) > Mathf.Abs(normalizedDirection.y))
			{
				axisDirection = new Vector2Int((int)Mathf.Sign(normalizedDirection.x), 0);
			}
			else
			{
				axisDirection = new Vector2Int(0, (int)Mathf.Sign(normalizedDirection.y * -1));
			}

			_moveItem?.Invoke(_coords, axisDirection);
		}
		else
		{
			m_image.ScaleOutEffect();
		}
	}
}