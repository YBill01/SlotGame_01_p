using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIFloatingStringEffect : MonoBehaviour
{
	[SerializeField]
	private Vector2 m_direction = Vector2.up;
	[SerializeField]
	private float m_length = 100.0f;
	[SerializeField]
	private float m_duration = 1.0f;

	[Space]
	[SerializeField]
	private GameObject m_particlePrefab;
	[SerializeField]
	private RectTransform m_container;

	private RectTransform _rectTransform;

	private Tween _tween;

	private void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
	}

	public void FloatingStringEffect(string text, Color color = default)
	{
		Vector2 startPoint = RandomUtils.PointInRect(_rectTransform.rect);
		Vector2 endPoint = startPoint + (m_direction * m_length);

		Vector3 inverseTransformPoint = m_container.InverseTransformPoint(_rectTransform.position);

		startPoint = new Vector2(startPoint.x + inverseTransformPoint.x, startPoint.y + inverseTransformPoint.y);
		endPoint = new Vector2(endPoint.x + inverseTransformPoint.x, endPoint.y + inverseTransformPoint.y);

		RectTransform particleRectTransform = (RectTransform)Instantiate(m_particlePrefab, startPoint, m_particlePrefab.transform.rotation, m_container).transform;
		particleRectTransform.anchoredPosition3D = startPoint;
		
		TMP_Text tmpText = particleRectTransform.gameObject.GetComponent<TMP_Text>();
		CanvasGroup canvasGroup = particleRectTransform.gameObject.GetComponent<CanvasGroup>();

		tmpText.text = text;
		tmpText.color = color;

		float distance = Vector2.Distance(endPoint, startPoint);

		_tween = particleRectTransform.DOAnchorPos(endPoint, m_duration)
			.From(startPoint)
			.SetEase(Ease.OutExpo)
			.OnUpdate(() =>
			{
				float distanceRatio = Vector2.Distance(particleRectTransform.anchoredPosition, startPoint) / distance;
				//float alpha = Math.Min(0.2f, distanceRatio) / 0.2f;
				//alpha = Math.Min(0.2f, distanceRatio) / 0.2f;
				//canvasGroup.alpha = Math.Min(0.2f, distanceRatio) / 0.2f;
				canvasGroup.alpha = (1.0f - Math.Max(0.95f, distanceRatio)) / (1.0f - 0.95f);
			})
			.OnComplete(() =>
			{
				Destroy(particleRectTransform.gameObject);
			});
	}

	private void OnDestroy()
	{
		_tween?.Kill();
	}
}